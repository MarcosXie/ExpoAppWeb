using AutoMapper;
using UExpo.Application.Utils;
using UExpo.Domain.Entities.Admins;
using UExpo.Domain.Entities.Chats.CallCenterChat;
using UExpo.Domain.Entities.Chats.Shared;
using UExpo.Domain.Entities.Users;
using UExpo.Domain.Exceptions;
using UExpo.Domain.FileStorage;
using UExpo.Domain.Translation;

namespace UExpo.Application.Services.Chats;

public class CallCenterChatService : ICallCenterChatService
{
	private readonly IFileStorageService _fileStorageService;
	private readonly ICallCenterChatRepository _repository;
	private readonly ICallCenterMessageRepository _callCenterMessageRepository;
	private readonly IUserRepository _userRepository;
	private readonly IAdminRepository _adminRepository;
	private readonly ITranslationService _translationService;
	private readonly IMapper _mapper;
	private readonly AuthUserHelper _authUserHelper;

	public CallCenterChatService(
		ICallCenterChatRepository repository,
		ICallCenterMessageRepository messageRepository,
		IUserRepository userRepository,
		IAdminRepository adminRepository,
		ITranslationService translationService,
		IFileStorageService fileStorageService,
		AuthUserHelper authUserHelper,
		IMapper mapper)
	{
		_fileStorageService = fileStorageService;
		_repository = repository;
		_callCenterMessageRepository = messageRepository;
		_userRepository = userRepository;
		_adminRepository = adminRepository;
		_translationService = translationService;
		_authUserHelper = authUserHelper;
		_mapper = mapper;
	}

	public async Task<Guid> CreateCallCenterChatAsync(ChatDto chat)
	{
		CallCenterChat dbChat = await _repository.GetByIdOrDefaultAsync(chat.Id) ??
			throw new NotFoundException("chat");

		IChatUser user = await GetChatUser(chat.UserId, dbChat);

		// Se o usuario esta acessando um chat que ja existia
		if (user is User && !dbChat.UserLang.Equals(chat.Lang))
		{
			dbChat.UserLang = user.Lang;

			await _repository.UpdateAsync(dbChat);
		}

		// Se é um atendente entrando no chat de um usuario
		else if (user is Admin && (!dbChat.AdminLang.Equals(chat.Lang) || dbChat.AdminId != chat.UserId))
		{
			dbChat.AdminId = chat.UserId;
			dbChat.AdminLang = user.Lang;

			await _repository.UpdateAsync(dbChat);
		}

		return dbChat.Id;
	}

	public async Task<(ReceiveMessageDto, bool)> AddMessageAsync(SendMessageDto message)
	{
		CallCenterChat? chat = await _repository.GetByIdOrDefaultAsync(message.RoomId);

		IChatUser senderUser = await GetChatUser(message.SenderId, chat);

		string senderLang = senderUser.Lang;
		string receiverLang = chat.UserLang.Equals(senderUser.Lang) ? chat.AdminLang : chat.UserLang;

		CallCenterMessage callCenterMessage = new()
		{
			ChatId = message.RoomId,
			ResponsedMessageId = message.ResponsedMessageId,
			SenderId = message.SenderId,
			SenderName = message.SenderName,
			SendedMessage = message.SendedMessage,
			SenderLang = senderLang,
			TranslatedMessage = await _translationService.TranslateText(message.SendedMessage, senderLang, receiverLang),
			ReceiverLang = receiverLang,
			Readed = false
		};

		if (!string.IsNullOrEmpty(message.FileName) && message.File != null)
		{
			string fileName = GetFileName(message.FileName, callCenterMessage.Id.ToString());
			callCenterMessage.FileName = message.FileName;
			callCenterMessage.File = await _fileStorageService.UploadFileAsync(message.File, fileName, FileStorageKeys.ChatFiles);
		}

		await _repository.AddMessageAsync(callCenterMessage);

		ReceiveMessageDto msgDto = new()
		{
			Id = callCenterMessage.Id,
			ResponsedMessageId = message.ResponsedMessageId,
			ResponsedMessage = callCenterMessage.ResponsedMessageId is not null ?
				_mapper.Map<ReceiveMessageDto>(
					await _callCenterMessageRepository.GetByIdAsync((Guid)callCenterMessage.ResponsedMessageId)
				) : null,
			RoomId = chat.Id.ToString(),
			SenderId = callCenterMessage.SenderId,
			SendedMessage = callCenterMessage.SendedMessage,
			TranslatedMessage = callCenterMessage.TranslatedMessage,
			File = callCenterMessage.File,
			FileName = callCenterMessage.FileName,
			SenderName = message.SenderName,
			SendedTime = callCenterMessage.CreatedAt,
			Readed = callCenterMessage.Readed,
			ReceiverId = (message.SenderId == chat.UserId ? chat.AdminId ?? Guid.NewGuid() : chat.UserId)
		};

		return (msgDto, senderUser is User);
	}

	public async Task UpdateChatAsync(ChatDto chat)
	{
		CallCenterChat? dbChat = await _repository.GetByIdOrDefaultAsync(chat.Id!);

		IChatUser user = await GetChatUser(chat.UserId, dbChat);

		if (user is User)
			dbChat!.UserLang = chat.Lang;
		else
			dbChat!.AdminLang = chat.Lang;

		await _repository.UpdateAsync(dbChat);
	}

	public async Task<List<BaseMessage>> GetMessagesByChatAsync(ChatDto chat)
	{
		var messages = await _repository.GetLastMessagesByChat(chat.Id);

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
			File = x.File,
			FileName = x.FileName,
			SendedTime = x.CreatedAt,
			Readed = x.Readed,
			Deleted = x.Deleted
		}).ToList();
	}

	public async Task<List<CallCenterChatResponseDto>> GetChatsAsync()
	{
		List<CallCenterChat> chats = await _repository.GetWithUsersAsync();

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
			Lang = isUser ? chat.User.Lang : chat.Admin?.Lang ?? "en",
			IsUser = isUser
		};
	}

	public async Task VisualizeMessagesAsync(ChatDto callCenterChat)
	{
		await _repository.VisualizeMessagesAsync(callCenterChat);
	}

	private async Task<IChatUser> GetChatUser(Guid id, CallCenterChat chat)
	{
		try
		{
			User user = await _userRepository.GetByIdAsync(id);
			user.Lang = user.Lang;
			return user;
		}
		catch (Exception)
		{
			Admin attendent = await _adminRepository.GetByIdAsync(id);
			attendent.Lang = attendent.Lang;
			return attendent;
		}
	}

	public async Task<CallCenterChatResponseDto> GetChatByUserIdAsync()
	{
		CallCenterChat chat = await _repository.GetOrCreateUserChatAsync(_authUserHelper.GetUser());

		CallCenterChatResponseDto responseDto = BuildCallCenterChatResponse(chat, true);

		responseDto.UserName = chat.Admin?.Name ?? "Waiting for attendent";

		return responseDto;
	}

	public async Task<int> GetNotReadedMessagesByUserId(Guid userId)
	{
		return await _repository.GetNotReadedMessagesByUserId(userId);
	}

	public async Task DeleteMessageAsync(DeleteMsgDto deleteMessage)
	{
		var message = await _callCenterMessageRepository.GetByIdAsync(deleteMessage.MsgId);

		message.Deleted = true;

		await _callCenterMessageRepository.UpdateAsync(message);
	}

	private static string GetFileName(string name, params string[] ids)
	{
		string prefix = string.Join('-', ids);

		return $"{prefix}-{Path.GetFileName(name)}";
	}
}
