using AutoMapper;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Carts;
using UExpo.Domain.Entities.Chats.CartChat;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class CartMessageRepository(UExpoDbContext context, IMapper mapper)
: BaseRepository<CartMessageDao, CartMessage>(context, mapper), ICartMessageRepository
{ 
}
