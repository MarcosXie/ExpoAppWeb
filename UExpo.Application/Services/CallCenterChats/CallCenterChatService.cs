using AutoMapper;
using UExpo.Domain.Admins;
using UExpo.Domain.CallCenterChat;
using UExpo.Domain.Translation;
using UExpo.Domain.Users;

namespace UExpo.Application.Services.CallCenterChats;

public class CallCenterChatService : ICallCenterChatService
{
    private ICallCenterChatRepository _repository;
    private IUserRepository _userRepository;
    private IAdminRepository _adminRepository;
    private ITranslationService _translationService;
    private IMapper _mapper;

    public CallCenterChatService(
        ICallCenterChatRepository repository,
        IUserRepository userRepository,
        IAdminRepository adminRepository,
        ITranslationService translationService,
        IMapper mapper)
    {
        _repository = repository;
        _userRepository = userRepository;
        _adminRepository = adminRepository;
        _translationService = translationService;
        _mapper = mapper;
    }

    public async Task<(Guid, string)> CreateCallCenterChatAsync(CallCenterChatDto chat)
    {
        var dbChat = await _repository.GetByIdOrDefaultAsync(chat.Id);

        // Se o chat ja existia
        if (dbChat is not null)
        {
            // Se o usuario esta acessando um chat que ja existia
            if (chat.Id.Equals(chat.UserId))
            {
                if(!dbChat.UserLang.Equals(chat.Lang))
                {
                    dbChat.UserLang = chat.Lang;

                    await _repository.UpdateAsync(dbChat);
                }
                var dbUser = await _userRepository.GetByIdAsync(chat.UserId);

                return (dbChat.Id, dbUser.Name);
            }
            // Se é um atendente entrando no chat de um usuario

            dbChat.AttendentId = chat.UserId;
            dbChat.AttendentLang = chat.Lang;

            await _repository.UpdateAsync(dbChat);

            var attendent = await _adminRepository.GetByIdAsync(chat.UserId);

            return (dbChat.Id, attendent.Name);
        }

        // Se é um novo chat sendo criado pelo usuario
        CallCenterChat callCenterChat = new()
        {
            UserId = chat.UserId,
            UserLang = chat.Lang,
        };

        var user = await _userRepository.GetByIdAsync(chat.UserId);

        return (await _repository.CreateAsync(callCenterChat), user.Name);
    }

    public async Task<CallCenterReceiveMessageDto> AddMessageAsync(CallCenterSendMessageDto message)
    {
        var chat = await _repository.GetByIdOrDefaultAsync(message.RoomId);

        var senderUser = await GetChatUser(message.SenderId, chat);

        var senderLang = senderUser.Language;
        var receiverLang = chat.UserLang.Equals(senderUser.Language) ? chat.AttendentLang : chat.UserLang;

        CallCenterMessage callCenterMessage = new()
        {
            ChatId = message.RoomId,
            SenderId = message.SenderId,
            SendedMessage = message.SendedMessage,
            SenderLang = senderLang,
            SenderName = senderUser.Name,
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
            SenderName = senderUser.Name,
            Readed = callCenterMessage.Readed
        };

        return msgDto;
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
            attendent.Language = chat.AttendentLang;
            return attendent;
        }
    }

    public async Task UpdateChatAsync(CallCenterChatDto chat)
    {
        var dbChat = await _repository.GetByIdOrDefaultAsync(chat.Id);

        if (chat.Id.Equals(chat.UserId)) 
            dbChat.UserLang = chat.Lang;
        else 
            dbChat.AttendentLang = chat.Lang;

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
            Readed = x.Readed
         }).ToList();
    }
}
