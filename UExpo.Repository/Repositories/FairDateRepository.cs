using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;
using UExpo.Domain.FairDates;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class FairDateRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<FairDateDao, FairDate>(context, mapper), IFairDateRepository
{
    public Task<bool> HasDateInRangeAsync(DateTime beginDate, DateTime endDate)
    {
        return Database.AnyAsync(x => 
            (x.BeginDate > beginDate && x.BeginDate < endDate) ||
            (x.BeginDate > beginDate && x.BeginDate < endDate));
    }
}
