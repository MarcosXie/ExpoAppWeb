using AutoMapper;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Calendar.Segments;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class CalendarSegmentRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<CalendarSegmentDao, CalendarSegment>(context, mapper), ICalendarSegmentRepository
{
}
