using AutoMapper;
using UExpo.Domain.Dao;
using UExpo.Domain.FairDates;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class FairDateRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<FairDateDao, FairDate>(context, mapper), IFairDateRepository
{
}
