using AutoMapper;
using UExpo.Domain.Attendents;
using UExpo.Repository.Context;
using UExpo.Repository.Dao;

namespace UExpo.Repository.Repositories;

public class AttendentRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<AttendentDao, Attendent>(context, mapper), IAttendentRepository;
