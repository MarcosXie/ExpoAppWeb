using AutoMapper;
using UExpo.Domain.Attendents;
using UExpo.Domain.CallCenterChat;
using UExpo.Domain.Users;

namespace UExpo.Application.Services.CallCenterChats;

public class CallCenterChatService : ICallCenterChatService
{
    private ICallCenterChatRepository _repository;
    private IUserRepository _userRepository;
    private IAttendentRepository _attendentRepository;
    private IMapper _mapper;

    public CallCenterChatService(
        ICallCenterChatRepository repository,
        IUserRepository userRepository,
        IAttendentRepository attendentRepository,
        IMapper mapper)
    {
        _repository = repository;
        _userRepository = userRepository;
        _attendentRepository = attendentRepository;
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

            var attendent = await _attendentRepository.GetByIdAsync(chat.UserId);

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

        var receiverUser = await GetChatUser(message, chat);

        CallCenterMessage callCenterMessage = new()
        {
            ChatId = message.RoomId,
            SenderId = message.SenderId,
            SendedMessage = message.SendedMessage,
            SenderLang = message.Lang,
            TranslatedMessage = TranslateAsync(message.SendedMessage, message.Lang, receiverUser.Language),
            ReceiverLang = receiverUser.Language,
            Readed = false
        };

        await _repository.AddMessageAsync(callCenterMessage);

        CallCenterReceiveMessageDto msgDto = new()
        {
            RoomId = chat.Id.ToString(),
            SenderId = callCenterMessage.SenderId,
            SendedMessage = callCenterMessage.SendedMessage,
            TranslatedMessage = callCenterMessage.TranslatedMessage,
            SenderName = receiverUser.Name,
            Readed = callCenterMessage.Readed
        };

        return msgDto;
    }

    private async Task<IChatUser> GetChatUser(CallCenterSendMessageDto message, CallCenterChat chat)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(message.SenderId);
            user.Language = chat.UserLang;
            return user;
        }
        catch (Exception)
        {
            var attendent = await _attendentRepository.GetByIdAsync(message.SenderId);
            attendent.Language = chat.AttendentLang;
            return attendent;
        }
    }

    private string TranslateAsync(string sendedMessage, string sourceLang, string targetLang)
    {
        return sendedMessage;
    }
}
