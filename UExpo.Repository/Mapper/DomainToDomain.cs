using AutoMapper;
using UExpo.Domain.Entities.Calendar;
using UExpo.Domain.Entities.Fairs;
using UExpo.Domain.Entities.Fairs.Segments;

namespace UExpo.Repository.Mapper;

public class DomainToDomain : Profile
{
    public DomainToDomain()
    {
        CreateMap<Fair, CalendarFair>().ReverseMap();
        CreateMap<Segment, CalendarSegment>().ReverseMap();
    }
}
