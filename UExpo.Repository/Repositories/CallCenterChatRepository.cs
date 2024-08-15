using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Admins;
using UExpo.Domain.Authentication;
using UExpo.Domain.CallCenterChat;
using UExpo.Domain.Dao;
using UExpo.Domain.Exceptions;
using UExpo.Domain.Users;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class CallCenterChatRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<CallCenterChatDao, CallCenterChat>(context, mapper), ICallCenterChatRepository
{
    public async Task<Guid> AddMessageAsync(CallCenterMessage message, CancellationToken cancellationToken = default)
    {
        var callCenter = await Database
            .Include(x => x.Messages)
            .FirstOrDefaultAsync(x =>
                x.Id == message.ChatId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(message.ChatId.ToString());

        message.CreatedAt = DateTime.Now;

        var messageDao = Mapper.Map<CallCenterMessageDao>(message);

        messageDao.ChatId = callCenter.Id;
        Context.CallCenterMessages.Add(messageDao);

        try
        {
            await Context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Console.Write(ex.ToString());
        }

        return messageDao.Id;
    }

    public async Task<CallCenterChat?> GetByUserIdAsync(Guid id)
    {
        var chat = await Database.FirstOrDefaultAsync(x => x.UserId == id);

        return chat is null ? default : Mapper.Map<CallCenterChat>(chat);
    }

    public async Task<List<CallCenterMessage>> GetLastMessagesByChat(Guid id)
    {
        var chat = await Database.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        var messages = await Context.CallCenterMessages
            .Where(x => x.ChatId == chat.Id)
            .OrderByDescending(x => x.CreatedAt)
            .Take(30)
            .ToListAsync();

        return [.. messages.Select(Mapper.Map<CallCenterMessage>).OrderBy(x => x.CreatedAt)];
    }

    public async Task<List<CallCenterChat>> GetWithUsersAsync()
    {
        var chats = await Database.AsNoTracking()
            .Include(x => x.User)
            .Select(chat => new CallCenterChat
            {
                Id = chat.Id,
                AdminId = chat.AdminId,
                AdminLang = chat.AdminLang,
                CreatedAt = chat.CreatedAt,
                UpdatedAt = chat.UpdatedAt,
                UserId = chat.UserId,
                User = Mapper.Map<User>(chat.User),
                UserLang = chat.UserLang,
                NotReadedMessages = chat.Messages.Where(x => !x.Readed && x.SenderId == chat.UserId).Count()
            })
            .ToListAsync();

        return chats;
    }

    public async Task<CallCenterChat> GetOrCreateUserChatAsync(AuthenticatedUser authenticatedUser)
    {
        var userId = Guid.Parse(authenticatedUser.Id);

        var chat = await Database.AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Admin)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (chat is null)
        {
            CallCenterChat callCenterChat = new()
            {
                UserId = userId,
                UserLang = "en", //TODO: Deixar dinamico
            };

            await CreateAsync(callCenterChat);

            chat = await Database.AsNoTracking()
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == Guid.Parse(authenticatedUser.Id));
        }

        return new CallCenterChat
        {
            Id = chat!.Id,
            AdminId = chat.AdminId,
            Admin = Mapper.Map<Admin>(chat.Admin),
            AdminLang = chat.AdminLang,
            CreatedAt = chat.CreatedAt,
            UpdatedAt = chat.UpdatedAt,
            UserId = chat.UserId,
            User = Mapper.Map<User>(chat.User),
            UserLang = chat.UserLang,
            NotReadedMessages = chat.Messages.Where(x => !x.Readed && x.SenderId != Guid.Parse(authenticatedUser.Id)).Count()
        };
    }

    public async Task VisualizeMessagesAsync(CallCenterChatDto callCenterChat)
    {
        var chat = await Database.Include(x => x.Messages)
            .FirstAsync(x => x.Id == callCenterChat.Id);


        var notReadedMessages = chat.Messages
            .Where(x => x.SenderId != callCenterChat.UserId && !x.Readed).ToList();


        foreach (var message in notReadedMessages)
            message.Readed = true;


        await Context.SaveChangesAsync();
    }

    public async Task<int> GetNotReadedMessagesByChatId(Guid roomId)
    {
        var chat = await Database.Include(x => x.Messages).FirstOrDefaultAsync(x => x.Id == roomId);

        return await Context.CallCenterMessages.CountAsync(x => !x.Readed && x.ChatId == roomId && x.SenderId != chat!.UserId); ;
    }

    public async Task<int> GetNotReadedMessagesByUserId(Guid userId)
    {
        var chat = await Database.Include(x => x.Messages).FirstOrDefaultAsync(x => x.UserId == userId);

        return await Context.CallCenterMessages.CountAsync(x => !x.Readed && x.ChatId == chat!.Id && x.SenderId != chat!.UserId);
    }
}
