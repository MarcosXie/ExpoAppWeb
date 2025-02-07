using AutoMapper;
using ExpoApp.Domain.Dao;
using ExpoApp.Domain.Entities.Momento;
using ExpoApp.Repository.Context;
using ExpoShared.Repository.Repositories;

namespace ExpoApp.Repository.Repositories;

public class MomentoRepository(ExpoAppDbContext context, IMapper mapper) 
	: BaseRepository<MomentoDao, Momento>(context, mapper), IMomentoRepository;