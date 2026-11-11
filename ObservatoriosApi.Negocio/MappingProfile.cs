using AutoMapper;
using ObservatoriosApi.Entidades;
using ObservatoriosApi.Entidades.Dtos;

namespace ObservatoriosApi.Negocio;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Observatorio, ObservatorioDto>();
        CreateMap<ObservatorioCrearDto, Observatorio>();
        CreateMap<ObservatorioActualizarDto, Observatorio>();
        CreateMap<ReservaObservacion, ReservaObservacionDto>()
            .ForMember(d => d.Observatorio, o => o.MapFrom(s => s.Observatorio.Nombre));
        CreateMap<ReservaObservacionCrearDto, ReservaObservacion>()
            .ForMember(d => d.Estado, o => o.MapFrom(_ => "Pendiente"));
        CreateMap<ReservaObservacionActualizarDto, ReservaObservacion>();
    }
}
