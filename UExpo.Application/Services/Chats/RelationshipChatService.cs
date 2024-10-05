using AutoMapper;
using UExpo.Application.Utils;
using UExpo.Domain.Entities.Chats.RelationshipChat;
using UExpo.Domain.Entities.Chats.Shared;
using UExpo.Domain.Entities.Relationships;
using UExpo.Domain.Entities.Users;
using UExpo.Domain.Translation;

namespace UExpo.Application.Services.Chats;

public class RelationshipChatService : IRelationshipChatService
{
	private IRelationshipRepository _relationshipRepository;
	private IRelationshipMessageRepository _relationshipMessageRepository;
	private IUserRepository _userRepository;
	private ITranslationService _translationService;
	private IMapper _mapper;
	private AuthUserHelper _authUserHelper;

	public RelationshipChatService(
		IRelationshipRepository relationshipRepository,
		IRelationshipMessageRepository relationshipMessage,
		IUserRepository userRepository,
		ITranslationService translationService,
		IMapper mapper,
		AuthUserHelper authUserHelper
		)
	{
		_relationshipRepository = relationshipRepository;
		_relationshipMessageRepository = relationshipMessage;
		_userRepository = userRepository;
		_translationService = translationService;
		_mapper = mapper;
		_authUserHelper = authUserHelper;
	}

	public async Task<ReceiveMessageDto> AddMessageAsync(SendMessageDto message)
	{
		Relationship? chat = await _relationshipRepository.GetByIdAsync(message.RoomId);

		var isSupplier = chat.SupplierUserId == message.SenderId;

		var senderLang = isSupplier ? chat.SupplierLang : chat.BuyerLang;
		var receiverLang = isSupplier ? chat.BuyerLang : chat.SupplierLang;

		RelationshipMessage relationshipMessage = new()
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

		await _relationshipRepository.AddMessageAsync(relationshipMessage);

		ReceiveMessageDto msgDto = new()
		{
			Id = relationshipMessage.Id,
			ResponsedMessageId = relationshipMessage.ResponsedMessageId,
			ResponsedMessage = relationshipMessage.ResponsedMessageId is not null ? 
				_mapper.Map<ReceiveMessageDto>(
					await _relationshipMessageRepository.GetByIdAsync((Guid)relationshipMessage.ResponsedMessageId)
				) : null,
			RoomId = chat.Id.ToString(),
			SenderId = relationshipMessage.SenderId,
			SendedMessage = relationshipMessage.SendedMessage,
			TranslatedMessage = relationshipMessage.TranslatedMessage,
			SenderName = message.SenderName,
			SendedTime = relationshipMessage.CreatedAt,
			ReceiverId = isSupplier ? chat.BuyerUserId : chat.SupplierUserId,
			Readed = relationshipMessage.Readed,
		};

		return msgDto;
	}

	public async Task DeleteMessageAsync(DeleteMsgDto deleteMessage)
	{
		var message = await _relationshipMessageRepository.GetByIdAsync(deleteMessage.MsgId);

		message.Deleted = true;

		await _relationshipMessageRepository.UpdateAsync(message);
	}

	public async Task<List<BaseMessage>> GetMessagesByChatAsync(ChatDto joinChatDto)
	{
		var messages = await _relationshipRepository.GetLastMessagesByChat(joinChatDto.Id);

		return messages.Select(x => new BaseMessage
		{
			Id = x.Id,
			RoomId = x.ChatId.ToString(),
			ResponsedMessageId = x.ResponsedMessageId,
			ResponsedMessage = x.ResponsedMessage,
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
		List<BaseMessage> notReadedMessages = await _relationshipRepository.GetNotReadedMessages(userId);

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
		var dbChat = await _relationshipRepository.GetByIdAsync(chat.Id);

		var isSupplier = dbChat.SupplierUserId == chat.UserId;

		if (isSupplier)
			dbChat.SupplierLang = chat.Lang;
		else
			dbChat.BuyerLang = chat.Lang;

		await _relationshipRepository.UpdateAsync(dbChat);
	}

	public async Task VisualizeMessagesAsync(ChatDto chat)
	{
		await _relationshipRepository.VisualizeMessagesAsync(chat);
	}
}

