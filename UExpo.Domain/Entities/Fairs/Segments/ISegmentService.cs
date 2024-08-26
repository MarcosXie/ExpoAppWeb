namespace UExpo.Domain.Entities.Fairs.Segments;

public interface ISegmentService
{
    Task<List<SegmentResponseDto>> GetByFairAsync(Guid fairId);
    Task<Guid> CreateAsync(SegmentDto segment);
    Task DeleteAsync(Guid id);
}
