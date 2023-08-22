using AutoMapper;
using tareas_mvc.Entidades;
using tareas_mvc.Models;

namespace tareas_mvc.Servicios
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Tarea, TareaDTO>();
        }
    }
}
