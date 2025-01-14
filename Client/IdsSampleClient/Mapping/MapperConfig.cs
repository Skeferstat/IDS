using AutoMapper;

namespace IdsSampleClient.Mapping;
internal class MapperConfig
{
    public static Mapper Get()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BasketReceiveMappingProfile>();
        });
        var mapper = new Mapper(config);
        return mapper;
    }
}
