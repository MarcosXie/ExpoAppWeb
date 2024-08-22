using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;
using UExpo.Domain.Fairs.Segments;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class SegmentRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<SegmentDao, Segment>(context, mapper), ISegmentRepository
{
    public async Task<bool> AnyWithSameNameInFairAsync(SegmentDto segment)
    {
        return await Database.AnyAsync(x => x.Name == segment.Name && x.FairId == segment.FairId);
    }

    public async Task<List<Segment>> GetByFairIdAsync(Guid fairId)
    {
        var fairs = await Database.Where(x => x.FairId.Equals(fairId)).ToListAsync();

        return Mapper.Map<List<Segment>>(fairs);
    }
}
