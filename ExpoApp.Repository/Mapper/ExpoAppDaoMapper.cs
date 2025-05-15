using AutoMapper;
using ExpoApp.Domain.Dao;
using ExpoApp.Domain.Dao.Wed;
using ExpoApp.Domain.Entities.Momento;
using ExpoApp.Domain.Entities.Wed;

namespace ExpoApp.Repository.Mapper;

public class ExpoAppDaoMapper : Profile
{
    public ExpoAppDaoMapper()
    {
		// Momento
		CreateMap<MomentoDao, Momento>().ReverseMap();
		CreateMap<PresentDao, Present>().ReverseMap();
	}
}
