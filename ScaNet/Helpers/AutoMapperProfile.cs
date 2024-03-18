
using AutoMapper;
using EndPointFinder.Models.ApiScanerModels;
using EndPointFinder.Models.EndpointScanerModels;

namespace ScaNet.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // ApiScanner Mapper
        CreateMap<ApiScanerModels, ApiScanerRootModels>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Apis, opt => opt.MapFrom(src => src.Apis))
            .ForMember(dest => dest.Keys, opt => opt.MapFrom(src => src.Keys));

        CreateMap<ApiScanerModels.ApiResponseModels, ApiModels>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RequestUrl, opt => opt.MapFrom(src => src.RequestUrl))
            .ForMember(dest => dest.InitiatorUrl, opt => opt.MapFrom(src => src.InitiatorUrl));

        CreateMap<ApiScanerModels.KeyResponseModels, KeyModels>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RequestUrl, opt => opt.MapFrom(src => src.RequestUrl))
            .ForMember(dest => dest.InitiatorUrl, opt => opt.MapFrom(src => src.InitiatorUrl));

        CreateMap<ApiScanerRootModels, ApiScanerModels>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Apis, opt => opt.MapFrom(src => src.Apis))
            .ForMember(dest => dest.Keys, opt => opt.MapFrom(src => src.Keys));

        CreateMap<ApiModels, ApiScanerModels.ApiResponseModels>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RequestUrl, opt => opt.MapFrom(src => src.RequestUrl))
            .ForMember(dest => dest.InitiatorUrl, opt => opt.MapFrom(src => src.InitiatorUrl));

        CreateMap<KeyModels, ApiScanerModels.KeyResponseModels>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RequestUrl, opt => opt.MapFrom(src => src.RequestUrl))
            .ForMember(dest => dest.InitiatorUrl, opt => opt.MapFrom(src => src.InitiatorUrl));


        // EndpointScanner Mapper
        CreateMap<EndpointScanerModels, EndpointScanerRootModels>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Endpoints, opt => opt.MapFrom(src => src.Endpoints))
            .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages));

        CreateMap<EndpointScanerModels.EndpointResponseModels, EndpointModels>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.Endpoint, opt => opt.MapFrom(src => src.Endpoint))
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));

        CreateMap<EndpointScanerRootModels, EndpointScanerModels>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Endpoints, opt => opt.MapFrom(src => src.Endpoints))
            .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages));

        CreateMap<EndpointModels, EndpointScanerModels.EndpointResponseModels>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.Endpoint, opt => opt.MapFrom(src => src.Endpoint))
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));
    }
}
