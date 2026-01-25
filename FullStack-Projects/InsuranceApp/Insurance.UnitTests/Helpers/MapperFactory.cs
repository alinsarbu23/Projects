using AutoMapper;
using Insurance.Application.Mappers;
using Insurance.Infrastructure.Mappers;

namespace Insurance.UnitTests.Helpers;

public static class MapperFactory
{
    public static IMapper Create()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<DtoMapperConfig>();
            cfg.AddProfile<DaoMapperConfig>();
        });

        return config.CreateMapper();
    }
}
