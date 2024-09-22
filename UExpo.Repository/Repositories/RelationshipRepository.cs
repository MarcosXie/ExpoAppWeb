using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Cart;
using UExpo.Domain.Entities.Chats.Shared;
using UExpo.Domain.Entities.Relationships;
using UExpo.Domain.Exceptions;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class RelationshipRepository(UExpoDbContext context, IMapper mapper)
	: BaseRepository<RelationshipDao, Relationship>(context, mapper), IRelationshipRepository
{
	public async Task<Guid> AddMessageAsync(BaseMessage message)
	{
		var callCenter = await Database
				.Include(x => x.Messages)
					.FirstOrDefaultAsync(x =>
						x.Id == message.ChatId)
					?? throw new NotFoundException(message.ChatId.ToString());

		message.CreatedAt = DateTime.Now;

		var messageDao = Mapper.Map<RelationshipMessageDao>(message);

		messageDao.ChatId = callCenter.Id;
		Context.RelationshipsMessages.Add(messageDao);

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

	public async Task<List<Relationship>> GetByUserIdAsync(Guid id)
	{
		var users = await Database
			.Include(x => x.BuyerUser)
				.ThenInclude(x => x.Images)
			.Include(x => x.SupplierUser)
				.ThenInclude(x => x.Images)
			.Include(x => x.Calendar)
			.Where(x => x.BuyerUserId == id || x.SupplierUserId == id)
			.ToListAsync();

		return Mapper.Map<List<Relationship>>(users);
	}

	public async Task<List<BaseMessage>> GetLastMessagesByChat(Guid id)
	{
		var chat = await Database.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

		var messages = await Context.RelationshipsMessages
			.Where(x => x.ChatId == chat!.Id)
			.OrderByDescending(x => x.CreatedAt)
			.Take(30)
			.ToListAsync();

		return [.. messages.Select(Mapper.Map<BaseMessage>).OrderBy(x => x.CreatedAt)];
	}

	public async Task<List<BaseMessage>> GetNotReadedMessages(Guid currentUserId)
	{
		var msgs = await Context.RelationshipsMessages.Where(x =>
			!x.Readed &&
			x.SenderId != currentUserId &&
			(x.Chat.BuyerUserId == currentUserId || x.Chat.SupplierUserId == currentUserId)
		).ToListAsync();

		return Mapper.Map<List<BaseMessage>>(msgs);
	}

	public async Task<int> GetNotReadedMessagesByChatId(Guid roomId, Guid? currentUserId = null)
	{
		var chat = await Database.Include(x => x.Messages).FirstOrDefaultAsync(x => x.Id == roomId);

		return await Context.CallCenterMessages.CountAsync(x => !x.Readed && x.ChatId == roomId && x.SenderId != currentUserId); ;
	}

	public async Task VisualizeMessagesAsync(ChatDto chatDto)
	{
		var chat = await Database.Include(x => x.Messages)
		.FirstAsync(x => x.Id == chatDto.Id);

		var notReadedMessages = chat.Messages
			.Where(x => x.SenderId != chatDto.UserId && !x.Readed).ToList();

		foreach (var message in notReadedMessages)
			message.Readed = true;

		await Context.SaveChangesAsync();
	}
}
