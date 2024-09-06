using AutoMapper;
using UExpo.Domain.Entities.Calendars.Fairs;
using UExpo.Domain.Entities.Calendars.Segments;
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
