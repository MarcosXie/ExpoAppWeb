using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Fairs.Segments;

public interface ISegmentRepository : IBaseRepository<SegmentDao, Segment>
{
    Task<bool> AnyWithSameNameInFairAsync(SegmentDto segment);
    Task<List<Segment>> GetByFairIdAsync(Guid fairId);
}
