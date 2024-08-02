using AutoMapper;
using UExpo.Domain.CallCenterChat;

namespace UExpo.Application.Services.CallCenterChats;

public class CallCenterChatService : ICallCenterChatService
{
    private ICallCenterChatRepository _repository;
    private IMapper _mapper;

    public CallCenterChatService(
        ICallCenterChatRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task CreateCallCenterChatAsync(CallCenterChatDto chat)
    {
        var callCenterChat = _mapper.Map<CallCenterChat>(chat);

        await _repository.CreateAsync(callCenterChat);
    }

    public async Task<CallCenterMessageDto> AddMessageAsync(CallCenterMessageDto message)
    {
        var callCenterMessage = _mapper.Map<CallCenterMessage>(message);

        await _repository.AddMessageAsync(callCenterMessage);

        message.TranslatedMessage = TranslateAsync(message.SendedMessage);

        return message;
    }

    private string TranslateAsync(string sendedMessage)
    {
        return sendedMessage;
    }
}
