using AutoMapper;
using UExpo.Domain.Calendar;
using UExpo.Domain.Dao;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class CalendarFairRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<CalendarFairDao, CalendarFair>(context, mapper), ICalendarFairRepository
{
}
