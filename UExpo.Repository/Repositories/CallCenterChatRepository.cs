using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Authentication;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Admins;
using UExpo.Domain.Entities.Chats.CallCenterChat;
using UExpo.Domain.Entities.Chats.Shared;
using UExpo.Domain.Entities.Users;
using UExpo.Domain.Exceptions;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class CallCenterChatRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<CallCenterChatDao, CallCenterChat>(context, mapper), ICallCenterChatRepository
{
    public async Task<Guid> AddMessageAsync(BaseMessage message)
    {
        CallCenterChatDao callCenter = await Database
            .Include(x => x.Messages)
            .FirstOrDefaultAsync(x => x.Id == message.ChatId)
            ?? throw new NotFoundException(message.ChatId.ToString());

        message.CreatedAt = DateTime.Now;

        CallCenterMessageDao messageDao = Mapper.Map<CallCenterMessageDao>(message);

        messageDao.ChatId = callCenter.Id;
        Context.CallCenterMessages.Add(messageDao);

        try
        {
            await Context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.Write(ex.ToString());
        }

        return messageDao.Id;
    }

    public async Task<CallCenterChat?> GetByUserIdAsync(Guid id)
    {
        CallCenterChatDao? chat = await Database.FirstOrDefaultAsync(x => x.UserId == id);

        return chat is null ? default : Mapper.Map<CallCenterChat>(chat);
    }

    public async Task<List<BaseMessage>> GetLastMessagesByChat(Guid id)
    {
        CallCenterChatDao? chat = await Database.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        List<CallCenterMessageDao> messages = await Context.CallCenterMessages
            .Where(x => x.ChatId == chat!.Id)
            .OrderByDescending(x => x.CreatedAt)
            .Take(30)
            .ToListAsync();

		List<BaseMessage> mappedMessages = [.. messages.Select(Mapper.Map<BaseMessage>).OrderBy(x => x.CreatedAt)];
		var responsedMessagesIds = messages
			.Where(x => x.ResponsedMessageId != null)
			.Select(x => x.ResponsedMessageId)
			.ToList();

		var responsedMessages = await Context.RelationshipsMessages
			.Where(x => responsedMessagesIds.Contains(x.Id))
			.ToListAsync();

		foreach (var msg in mappedMessages.Where(x => x.ResponsedMessageId != null))
		{
			var responsedMessage = responsedMessages.FirstOrDefault(x => x.Id == msg.ResponsedMessageId);

			if (responsedMessage != null)
			{
				msg.ResponsedMessage = Mapper.Map<BaseMessage>(responsedMessage);
			}
		}

		return mappedMessages;
	}

    public async Task<List<CallCenterChat>> GetWithUsersAsync()
    {
        List<CallCenterChat> chats = await Database.AsNoTracking()
            .Include(x => x.User)
			.Include(x => x.Admin)
            .Select(chat => new CallCenterChat
            {
                Id = chat.Id,
                AdminId = chat.AdminId,
				Admin = Mapper.Map<Admin>(chat.Admin),
                AdminLang = chat.Admin.Lang,
                CreatedAt = chat.CreatedAt,
                UpdatedAt = chat.UpdatedAt,
                UserId = chat.UserId,
                User = Mapper.Map<User>(chat.User),
                UserLang = chat.User.Lang,
                NotReadedMessages = chat.Messages.Where(x => !x.Readed && x.SenderId == chat.UserId).Count()
            })
            .ToListAsync();

        return chats;
    }

    public async Task<CallCenterChat> GetOrCreateUserChatAsync(AuthenticatedUser authenticatedUser)
    {
        Guid userId = authenticatedUser.Id;

        CallCenterChatDao? chat = await Database.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Admin)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (chat is null)
        {
			var user = await Context.Users.FirstOrDefaultAsync(x => x.Id == userId);

			CallCenterChat callCenterChat = new()
            {
                UserId = userId,
                UserLang = user!.Lang ?? "en",
                AdminLang = "en",
            };

            await CreateAsync(callCenterChat);

            chat = await Database.AsNoTracking()
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == authenticatedUser.Id);
        }

        return new CallCenterChat
        {
            Id = chat!.Id,
            AdminId = chat.AdminId,
            Admin = Mapper.Map<Admin>(chat.Admin),
            AdminLang = chat.Admin?.Lang ?? "en",
            CreatedAt = chat.CreatedAt,
            UpdatedAt = chat.UpdatedAt,
            UserId = chat.UserId,
            User = Mapper.Map<User>(chat.User),
            UserLang = chat.User.Lang,
            NotReadedMessages = chat.Messages.Where(x => !x.Readed && x.SenderId != authenticatedUser.Id).Count()
        };
    }

    public async Task VisualizeMessagesAsync(ChatDto callCenterChat)
    {
        CallCenterChatDao chat = await Database.Include(x => x.Messages)
            .FirstAsync(x => x.Id == callCenterChat.Id);


        List<CallCenterMessageDao> notReadedMessages = chat.Messages
            .Where(x => x.SenderId != callCenterChat.UserId && !x.Readed).ToList();


        foreach (CallCenterMessageDao? message in notReadedMessages)
            message.Readed = true;


        await Context.SaveChangesAsync();
    }

    public async Task<int> GetNotReadedMessagesByChatId(Guid roomId, Guid? currentUserId = null)
    {
        CallCenterChatDao? chat = await Database.Include(x => x.Messages).FirstOrDefaultAsync(x => x.Id == roomId);

        return await Context.CallCenterMessages.CountAsync(x => !x.Readed && x.ChatId == roomId && x.SenderId != chat!.UserId); ;
    }

    public async Task<int> GetNotReadedMessagesByUserId(Guid userId)
    {
        CallCenterChatDao? chat = await Database.Include(x => x.Messages).FirstOrDefaultAsync(x => x.UserId == userId);

		if (chat is null) return 0;

        return await Context.CallCenterMessages.CountAsync(x => !x.Readed && x.ChatId == chat!.Id && x.SenderId != chat!.UserId);
    }
}
