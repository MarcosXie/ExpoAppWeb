using AutoMapper;
using ExpoApp.Domain.Dao.Wed;
using ExpoApp.Domain.Entities.Wed;
using ExpoApp.Repository.Context;
using ExpoShared.Repository.Repositories;

namespace ExpoApp.Repository.Repositories;

public class PresentRepository(ExpoAppDbContext context, IMapper mapper)  
	: BaseRepository<PresentDao, Present>(context, mapper), IPresentRepository
{
}
