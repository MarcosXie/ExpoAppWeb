using AutoMapper;
using UExpo.Domain.Calendar;
using UExpo.Domain.Dao;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class CalendarRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<CalendarDao, Calendar>(context, mapper), ICalendarRepository
{
}
