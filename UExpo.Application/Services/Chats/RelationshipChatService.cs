using AutoMapper;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using UExpo.Application.Utils;
using UExpo.Domain.Entities.Chats.RelationshipChat;
using UExpo.Domain.Entities.Chats.Shared;
using UExpo.Domain.Entities.Relationships;
using UExpo.Domain.Entities.Users;
using UExpo.Domain.Translation;
using UExpo.Domain.FileStorage;

namespace UExpo.Application.Services.Chats;

public class RelationshipChatService : IRelationshipChatService
{
	private readonly IRelationshipRepository _relationshipRepository;
	private readonly IRelationshipMessageRepository _relationshipMessageRepository;
	private readonly IUserRepository _userRepository;
	private readonly ITranslationService _translationService;
	private readonly IFileStorageService _fileStorageService;
	private readonly IMapper _mapper;
	private readonly AuthUserHelper _authUserHelper;

	public RelationshipChatService(
		IRelationshipRepository relationshipRepository,
		IRelationshipMessageRepository relationshipMessage,
		IUserRepository userRepository,
		ITranslationService translationService,
		IFileStorageService fileStorageService,
		IMapper mapper,
		AuthUserHelper authUserHelper
		)
	{
		_relationshipMessageRepository = relationshipMessage;
		_relationshipRepository = relationshipRepository;
		_translationService = translationService;
		_fileStorageService = fileStorageService;
		_authUserHelper = authUserHelper;
		_userRepository = userRepository;
		_mapper = mapper;
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

		if (!string.IsNullOrEmpty(message.FileName) && message.File != null)
		{
			string fileName = GetFileName(message.FileName, chat.Id.ToString());

			using var stream = new MemoryStream(message.File);

			IFormFile formFile = new FormFile(stream, 0, message.File.Length, "file", message.FileName)
			{
				Headers = new HeaderDictionary(),
				ContentType = "application/octet-stream"
			};

			relationshipMessage.FileName = Path.GetFileName(formFile.FileName);
			relationshipMessage.File = await _fileStorageService.UploadFileAsync(formFile, fileName, FileStorageKeys.ChatFiles);
		}

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
			File = relationshipMessage.File,
			FileName = relationshipMessage.FileName,
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
			File = x.File,
			FileName = x.FileName,
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

	private static string GetFileName(string name, params string[] ids)
	{
		string prefix = string.Join('-', ids);

		return $"{prefix}-{Path.GetFileName(name)}";
	}
}

