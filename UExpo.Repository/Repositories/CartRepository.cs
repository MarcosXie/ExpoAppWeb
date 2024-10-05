using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Carts;
using UExpo.Domain.Entities.Chats.Shared;
using UExpo.Domain.Exceptions;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class CartRepository(UExpoDbContext context, IMapper mapper)
	: BaseRepository<CartDao, Cart>(context, mapper), ICartRepository
{
	#region Chat
	public async Task<Guid> AddMessageAsync(BaseMessage message)
	{
		var callCenter = await Database
				.Include(x => x.Messages)
					.FirstOrDefaultAsync(x =>
						x.Id == message.ChatId)
					?? throw new NotFoundException(message.ChatId.ToString());

		message.CreatedAt = DateTime.Now;

		var messageDao = Mapper.Map<CartMessageDao>(message);

		messageDao.ChatId = callCenter.Id;
		Context.CartMessages.Add(messageDao);

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

	public async Task<List<BaseMessage>> GetLastMessagesByChat(Guid id)
	{
		var chat = await Database.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

		var messages = await Context.CartMessages
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

	public async Task<List<BaseMessage>> GetNotReadedMessages(Guid currentUserId)
	{
		var msgs = await Context.CartMessages.Where(x =>
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
	#endregion
	public async Task<Cart> GetByIdDetailedAsync(Guid id)
	{
		var entity = await Database
			.Include(x => x.SupplierUser)
			.Include(x => x.BuyerUser)
			.Include(x => x.Items)
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id!.Equals(id));

		return entity is null
			? throw new Exception($"{nameof(CartDao)} com id = {id}")
		: Mapper.Map<Cart>(entity);
	}

	public async Task<List<Cart>> GetDetailedAsync(Guid userId)
	{
		var carts = await Database
			.Include (x => x.BuyerUser)
			.Include(x => x.SupplierUser)
			.Include(x => x.Items)
			.AsSplitQuery()
			.Where(x => x.SupplierUserId == userId || x.BuyerUserId == userId && x.Status != CartStatus.Building)
			.ToListAsync();

		return Mapper.Map<List<Cart>>(carts);
	}

	public async Task<List<CartItem>> GetItemsAsync(Guid buyerId, Guid supplierId)
	{
		var items = (await Database
			.Include(x => x.Items)
			.FirstOrDefaultAsync(x =>
				x.Status == CartStatus.Building &&
				x.BuyerUserId == buyerId &&
				x.SupplierUserId == supplierId))
			?.Items;

		return Mapper.Map<List<CartItem>>(items);
	}

	public async Task<string> GetNextCartNoAsync(char separator)
	{
		var currentYear = (DateTime.Now.Year % 100).ToString();

		var cart = await Database
			.Where(x => x.CartNo.EndsWith(currentYear))
			.OrderByDescending(x => x.CartNo)
			.FirstOrDefaultAsync();

		

		return int.TryParse(cart?.CartNo?.Split(separator)[0], out int result) ? (result + 1).ToString() : "1";
	}
}
