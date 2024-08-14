using AutoMapper;
using UExpo.Application.Utils;
using UExpo.Domain.Admins;
using UExpo.Domain.CallCenterChat;
using UExpo.Domain.Exceptions;
using UExpo.Domain.Translation;
using UExpo.Domain.Users;

namespace UExpo.Application.Services.CallCenterChats;

public class CallCenterChatService : ICallCenterChatService
{
    private ICallCenterChatRepository _repository;
    private IUserRepository _userRepository;
    private IAdminRepository _adminRepository;
    private ITranslationService _translationService;
    private AuthUserHelper _authUserHelper;
    private IMapper _mapper;

    public CallCenterChatService(
        ICallCenterChatRepository repository,
        IUserRepository userRepository,
        IAdminRepository adminRepository,
        ITranslationService translationService,
        AuthUserHelper authUserHelper,
        IMapper mapper)
    {
        _repository = repository;
        _userRepository = userRepository;
        _adminRepository = adminRepository;
        _translationService = translationService;
        _authUserHelper = authUserHelper;
        _mapper = mapper;
    }

    public async Task<Guid> CreateCallCenterChatAsync(CallCenterChatDto chat)
    {
        var dbChat = await _repository.GetByIdOrDefaultAsync(chat.Id) ??
            throw new NotFoundException("chat");

        var user = await GetChatUser(chat.UserId, dbChat);

        // Se o usuario esta acessando um chat que ja existia
        if (user is User && !dbChat.UserLang.Equals(chat.Lang))
        {
            dbChat.UserLang = chat.Lang;

            await _repository.UpdateAsync(dbChat);
        } 

        // Se é um atendente entrando no chat de um usuario
        else if (user is Admin && (!dbChat.AdminLang.Equals(chat.Lang) || dbChat.AdminId != chat.UserId))
        {
            dbChat.AdminId = chat.UserId;
            dbChat.AdminLang = chat.Lang;

            await _repository.UpdateAsync(dbChat);
        }

        return dbChat.Id;
    }

    public async Task<(CallCenterReceiveMessageDto, bool)> AddMessageAsync(CallCenterSendMessageDto message)
    {
        var chat = await _repository.GetByIdOrDefaultAsync(message.RoomId);

        var senderUser = await GetChatUser(message.SenderId, chat);

        var senderLang = senderUser.Language;
        var receiverLang = chat.UserLang.Equals(senderUser.Language) ? chat.AdminLang : chat.UserLang;

        CallCenterMessage callCenterMessage = new()
        {
            ChatId = message.RoomId,
            SenderId = message.SenderId,
            SenderName = message.SenderName,
            SendedMessage = message.SendedMessage,
            SenderLang = senderLang,
            TranslatedMessage = await _translationService.TranslateText(message.SendedMessage, senderLang, receiverLang),
            ReceiverLang = receiverLang,
            Readed = false
        };

        await _repository.AddMessageAsync(callCenterMessage);

        CallCenterReceiveMessageDto msgDto = new()
        {
            RoomId = chat.Id.ToString(),
            SenderId = callCenterMessage.SenderId,
            SendedMessage = callCenterMessage.SendedMessage,
            TranslatedMessage = callCenterMessage.TranslatedMessage,
            SenderName = message.SenderName,
            SendedTime = callCenterMessage.CreatedAt,
            Readed = callCenterMessage.Readed
        };

        return (msgDto, senderUser is User);
    }

    public async Task UpdateChatAsync(CallCenterChatDto chat)
    {
        var dbChat = await _repository.GetByIdOrDefaultAsync(chat.Id!);

        var user = await GetChatUser(chat.UserId, dbChat);

        if (user is User)
            dbChat!.UserLang = chat.Lang;
        else
            dbChat!.AdminLang = chat.Lang;

        await _repository.UpdateAsync(dbChat);
    }

    public async Task<List<CallCenterReceiveMessageDto>> GetMessagesByChatAsync(CallCenterChatDto chat)
    {
        var messages = await _repository.GetLastMessagesByChat(chat.Id);

        return messages.Select(x => new CallCenterReceiveMessageDto
        {
            RoomId = x.ChatId.ToString(),
            SenderId = x.SenderId,
            SendedMessage = x.SendedMessage,
            SenderName = x.SenderName,
            TranslatedMessage = x.TranslatedMessage,
            SendedTime = x.CreatedAt,
            Readed = x.Readed
        }).ToList();
    }

    public async Task<List<CallCenterChatResponseDto>> GetChatsAsync()
    {
        var chats = await _repository.GetWithUsersAsync();

        return chats.Select(x => BuildCallCenterChatResponse(x, false)).ToList();
    }

    private static CallCenterChatResponseDto BuildCallCenterChatResponse(CallCenterChat chat, bool isUser = false)
    {
        return new CallCenterChatResponseDto()
        {
            Id = chat.Id,
            UserId = chat.UserId,
            UserName = chat.User.Name,
            UserEnterprise = chat.User.Enterprise,
            UserCountry = chat.User.Country,
            RegisterDate = chat.User.CreatedAt,
            NotReadedMessages = chat.NotReadedMessages,
            IsUser = isUser
        };
    }

    public async Task VisualizeMessagesAsync(CallCenterChatDto callCenterChat)
    {
        await _repository.VisualizeMessagesAsync(callCenterChat);
    }

    private async Task<IChatUser> GetChatUser(Guid id, CallCenterChat chat)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            user.Language = chat.UserLang;
            return user;
        }
        catch (Exception)
        {
            var attendent = await _adminRepository.GetByIdAsync(id);
            attendent.Language = chat.AdminLang;
            return attendent;
        }
    }

    public async Task<CallCenterChatResponseDto> GetChatByUserIdAsync()
    {
        var chat = await _repository.GetOrCreateUserChatAsync(_authUserHelper.GetUser());

        var responseDto = BuildCallCenterChatResponse(chat, true);

        responseDto.UserName = chat.Admin?.Name ?? "Waiting for attendent";

        return responseDto;
    }

    public async Task<(int, string)> GetNotReadedMessagesByRoomId(Guid roomId)
    {
        var chat = await _repository.GetByIdOrDefaultAsync(roomId);

        return (await _repository.GetNotReadedMessagesByChatId(roomId), chat!.UserId.ToString());
    }

    public async Task<int> GetNotReadedMessagesByUserId(string userId)
    {
        return await _repository.GetNotReadedMessagesByUserId(Guid.Parse(userId));
    }
}
