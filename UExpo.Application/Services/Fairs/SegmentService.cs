using AutoMapper;
using UExpo.Domain.Exceptions;
using UExpo.Domain.Fairs.Segments;

namespace UExpo.Application.Services.Fairs;

public class SegmentService : ISegmentService
{
    private ISegmentRepository _repository;
    private IMapper _mapper;

    public SegmentService(ISegmentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Guid> CreateAsync(SegmentDto segment)
    {
        await ValidateSegmentAsync(segment);

        return await _repository.CreateAsync(_mapper.Map<Segment>(segment));
    }

    public async Task DeleteAsync(Guid id)
    {
        await ValidateDeleteAsync(id);

        await _repository.DeleteAsync(id);
    }

    public async Task<List<SegmentResponseDto>> GetByFairAsync(Guid fairId)
    {
        var segments = await _repository.GetByFairIdAsync(fairId);

        return [.. segments.Select(_mapper.Map<SegmentResponseDto>).OrderByDescending(x => x.Name)];
    }

    private async Task ValidateSegmentAsync(SegmentDto segment)
    {
        if (await _repository.AnyWithSameNameInFairAsync(segment))
            throw new BadRequestException("Exist another place with same name in this fair!");
    }

    private async Task ValidateDeleteAsync(Guid id)
    {
        // TODO: Add validation fair not running
    }
}
