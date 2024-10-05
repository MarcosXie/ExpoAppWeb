using AutoMapper;
using UExpo.Application.Utils;
using UExpo.Domain.Entities.Carts;
using UExpo.Domain.Entities.Chats.CartChat;
using UExpo.Domain.Entities.Chats.Shared;
using UExpo.Domain.Entities.Users;
using UExpo.Domain.Translation;

namespace UExpo.Application.Services.Chats;

public class CartChatService : ICartChatService
{
	private ICartMessageRepository _cartMessageRepository;
	private ICartRepository _cartRepository;
	private IUserRepository _userRepository;
	private ITranslationService _translationService;
	private IMapper _mapper;
	private AuthUserHelper _authUserHelper;

	public CartChatService(
		ICartRepository cartRepository,
		ICartMessageRepository cartMessageRepository,
		IUserRepository userRepository,
		ITranslationService translationService,
		IMapper mapper,
		AuthUserHelper authUserHelper
		)
	{
		_cartMessageRepository = cartMessageRepository;
		_cartRepository = cartRepository;
		_userRepository = userRepository;
		_translationService = translationService;
		_mapper = mapper;
		_authUserHelper = authUserHelper;
	}

	public async Task<ReceiveMessageDto> AddMessageAsync(SendMessageDto message)
	{
		var chat = await _cartRepository.GetByIdAsync(message.RoomId);

		var isSupplier = chat.SupplierUserId == message.SenderId;

		var senderLang = isSupplier ? chat.SupplierLang : chat.BuyerLang;
		var receiverLang = isSupplier ? chat.BuyerLang : chat.SupplierLang;

		CartMessage cartMessage = new()
		{
			ChatId = message.RoomId,
			ResponsedMessageId = message.ResponsedMessageId,
			SenderId = message.SenderId,
			SenderName = message.SenderName,
			SendedMessage = message.SendedMessage,
			SenderLang = senderLang,
			TranslatedMessage = await _translationService.TranslateText(message.SendedMessage, senderLang, receiverLang),
			ReceiverLang = receiverLang,
			Readed = false,
		};

		await _cartRepository.AddMessageAsync(cartMessage);

		ReceiveMessageDto msgDto = new()
		{
			Id = cartMessage.Id,
			ResponsedMessageId = message.ResponsedMessageId,
			ResponsedMessage = cartMessage.ResponsedMessageId is not null ?
			_mapper.Map<ReceiveMessageDto>(
					await _cartMessageRepository.GetByIdAsync((Guid)cartMessage.ResponsedMessageId)
				) : null,
			RoomId = chat.Id.ToString(),
			SenderId = cartMessage.SenderId,
			SendedMessage = cartMessage.SendedMessage,
			TranslatedMessage = cartMessage.TranslatedMessage,
			SenderName = message.SenderName,
			SendedTime = cartMessage.CreatedAt,
			ReceiverId = isSupplier ? chat.BuyerUserId : chat.SupplierUserId,
			Readed = cartMessage.Readed
		};

		return msgDto;
	}

	public async Task DeleteMessageAsync(DeleteMsgDto deleteMessage)
	{
		var message = await _cartMessageRepository.GetByIdAsync(deleteMessage.MsgId);

		message.Deleted = true;

		await _cartMessageRepository.UpdateAsync(message);
	}

	public async Task<List<BaseMessage>> GetMessagesByChatAsync(ChatDto joinChatDto)
	{
		var messages = await _cartRepository.GetLastMessagesByChat(joinChatDto.Id);

		return messages.Select(x => new BaseMessage
		{
			Id = x.Id,
			ResponsedMessageId = x.ResponsedMessageId,
			ResponsedMessage = x.ResponsedMessage,
			RoomId = x.ChatId.ToString(),
			SenderId = x.SenderId,
			SendedMessage = x.SendedMessage,
			SenderName = x.SenderName,
			TranslatedMessage = x.TranslatedMessage,
			SendedTime = x.CreatedAt,
			Readed = x.Readed,
			Deleted = x.Deleted
		}).ToList();
	}

	public async Task<List<NotReadedMessagesDto>> GetNotReadedMessagesAsync(Guid userId)
	{
		List<BaseMessage> notReadedMessages = await _cartRepository.GetNotReadedMessages(userId);

		return notReadedMessages
			.GroupBy(x => x.ChatId)
			.Select(x => new NotReadedMessagesDto
			{
				ChatId = x.Key,
				NotReadedMessages = x.Count()
			}).ToList();
	}

	public async Task UpdateLangAsync(ChatDto chat)
	{
		var dbChat = await _cartRepository.GetByIdAsync(chat.Id);

		var isSupplier = dbChat.SupplierUserId == chat.UserId;

		if (isSupplier)
			dbChat.SupplierLang = chat.Lang;
		else
			dbChat.BuyerLang = chat.Lang;

		await _cartRepository.UpdateAsync(dbChat);
	}

	public async Task VisualizeMessagesAsync(ChatDto chat)
	{
		await _cartRepository.VisualizeMessagesAsync(chat);
	}
}
