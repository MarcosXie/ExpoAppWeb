using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;
using UExpo.Domain.Places;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class PlaceRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<PlaceDao, Place>(context, mapper), IPlaceRepository
{
    public async Task<bool> AnyWithSameYearAsync(int year, Guid? id) =>
        await Database.AnyAsync(x => x.Year == year && x.Id != id);
}
