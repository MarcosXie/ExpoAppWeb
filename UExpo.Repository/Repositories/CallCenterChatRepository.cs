using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.CallCenterChat;
using UExpo.Domain.Exceptions;
using UExpo.Repository.Context;
using UExpo.Repository.Dao;

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
        var messageDao = Mapper.Map<CallCenterMessageDao>(message);

        messageDao.ChatId = callCenter.Id;
        messageDao.CreatedAt = DateTime.Now;
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
            .OrderBy(x => x.CreatedAt)
            .Take(30)
            .ToListAsync();

        return messages.Select(x => Mapper.Map<CallCenterMessage>(x)).ToList();
    }

    public async Task<List<CallCenterChat>> GetWithUsersAsync()
    {
        var chats = await Database.AsNoTracking()
            .Include(x => x.User)
            .ToListAsync();

        return chats.Select(x => Mapper.Map<CallCenterChat>(x)).ToList();
    }

    public async Task VisualizeMessagesAsync(CallCenterChatDto callCenterChat)
    {
        var chat = await Database.Include(x => x.Messages)
            .FirstAsync(x => x.Id == callCenterChat.Id);


        var notReadedMessages = chat.Messages
            .Where(x => x.SenderId == callCenterChat.UserId && !x.Readed).ToList();


        foreach (var message in notReadedMessages)
            message.Readed = true;


        await Context.SaveChangesAsync();
    }
}
