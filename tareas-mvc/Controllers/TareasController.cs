using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tareas_mvc.Entidades;
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

        [HttpPost]
        public async Task<ActionResult<Tarea>> Post([FromBody] string titulo)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();

            var ordenMayor = await context.Tareas
                .Where(t => t.UsuarioCreacionId == usuarioId)
                .Select(t => t.Orden)
                .OrderByDescending(orden => orden)
                .FirstOrDefaultAsync();

            var tarea = new Tarea
            {
                Titulo = titulo,
                UsuarioCreacionId = usuarioId,
                FechaCreacion = DateTime.UtcNow,
                Orden = ordenMayor + 1,
                Descripcion = string.Empty,

            };

            context.Add(tarea);
            context.SaveChanges();

            return tarea;
        }
    }
}
