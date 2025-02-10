using AutoMapper;
using ExpoApp.Domain.Dao;
using ExpoApp.Domain.Entities.Momento;

namespace ExpoApp.Repository.Mapper;

public class ExpoAppDaoMapper : Profile
{
    public ExpoAppDaoMapper()
    {
		// Momento
		CreateMap<MomentoDao, Momento>().ReverseMap();
	}
}
