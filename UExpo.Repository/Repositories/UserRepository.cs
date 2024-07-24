using AutoMapper;
using UExpo.Domain.Users;
using UExpo.Repository.Context;
using UExpo.Repository.Dao;

namespace UExpo.Repository.Repositories;

public class UserRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<UserDao, User>(context, mapper), IUserRepository;
