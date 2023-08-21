using Microsoft.AspNetCore.Mvc;
using tareas_mvc.Servicios;

namespace tareas_mvc.Controllers
{
    [Route("/api/tareas")]
    public class TareasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IServicioUsuarios servicioUsuarios;

        public TareasController(ApplicationDbContext context, IServicioUsuarios servicioUsuarios)
        {
            this.context = context;
            this.servicioUsuarios = servicioUsuarios;
        }
    }
}
