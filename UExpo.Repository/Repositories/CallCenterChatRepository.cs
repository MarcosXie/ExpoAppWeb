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
    public override async Task<CallCenterChat?> GetByIdOrDefaultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await Database.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == id, cancellationToken: cancellationToken);

        return entity is null ? default : Mapper.Map<CallCenterChat>(entity);
    }

    public async Task<Guid> AddMessageAsync(CallCenterMessage message, CancellationToken cancellationToken = default)
    {
        var callCenter = await Database
            .Include(x => x.Messages)
            .FirstOrDefaultAsync(x => 
                x.UserId == message.ChatId, cancellationToken: cancellationToken)
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

    public async Task<List<CallCenterMessage>> GetLastMessagesByChat(Guid id)
    {
        var chat = await Database.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == id);
            
        var messages = await Context.CallCenterMessages
            .Where(x => x.ChatId == chat.Id)
            .OrderBy(x => x.CreatedAt)
            .Take(30)
            .ToListAsync();

        return messages.Select(x => Mapper.Map<CallCenterMessage>(x)).ToList();
    }
}
