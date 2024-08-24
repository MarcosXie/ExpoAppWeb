using AutoMapper;
using UExpo.Domain.Calendar;
using UExpo.Domain.Fairs;
using UExpo.Domain.Fairs.Segments;

namespace UExpo.Repository.Mapper;

public class DomainToDomain : Profile
{
    public DomainToDomain()
    {
        CreateMap<Fair, CalendarFair>().ReverseMap();
        CreateMap<Segment, CalendarSegment>().ReverseMap();
    }
}
