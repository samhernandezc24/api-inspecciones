using API.Inspecciones.Models;
using API.Inspecciones.ViewModels;
using API.Inspecciones.ViewModels.Reports;
using AutoMapper;

namespace API.Inspecciones.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // MAPPING PARA RETORNO DE DATASOURCE_EXPRESSION
            // I
            CreateMap<Inspeccion, InspeccionViewModel>();

            // U
            CreateMap<Unidad, UnidadViewModel>();

            // MAPPING PARA REPORTE
            // U
            CreateMap<Unidad, RepUnidad>()
                .ForMember(d => d.createdFecha, o => o.MapFrom(s => s.CreatedFechaNatural))
                .ForMember(d => d.updatedFecha, o => o.MapFrom(s => s.UpdatedFechaNatural));
        }
    }
}
