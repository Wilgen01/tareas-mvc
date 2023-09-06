using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tareas_mvc.Entidades;
using tareas_mvc.Models;
using tareas_mvc.Servicios;

namespace tareas_mvc.Controllers
{
    [Route("api/pasos")]
    public class PasosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IServicioUsuarios servicioUsuarios;

        public PasosController(ApplicationDbContext context, IServicioUsuarios servicioUsuarios)
        {
            this.context = context;
            this.servicioUsuarios = servicioUsuarios;
        }

        [HttpPost("{tareaId:int}")]
        public async Task<ActionResult<Paso>> Post (int tareaId, [FromBody]PasoCrearDTO pasoCrearDTO)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();

            var tarea = await context.Tareas.FirstOrDefaultAsync(t => t.Id == tareaId && t.UsuarioCreacionId == usuarioId);

            if (tarea == null)
            {
                return NotFound();
            }

            var existenPasos = await context.Pasos.AnyAsync();
            var ordenMayor = 0;
            if (existenPasos)
            {
                ordenMayor = await context.Pasos.Where(p => p.TareaId == tareaId).Select(p => p.Orden).MaxAsync();
            }

            var paso = new Paso();
            paso.TareaId = tareaId;
            paso.Orden = ordenMayor + 1;
            paso.Realizado = pasoCrearDTO.Realizado;
            paso.Descripcion = pasoCrearDTO.Descripcion;

            context.Add(paso);

            context.SaveChanges();


            return paso;

        }


    }
}
