using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tareas_mvc.Entidades;
using tareas_mvc.Models;
using tareas_mvc.Servicios;

namespace tareas_mvc.Controllers
{
    [Route("/api/tareas")]
    public class TareasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IServicioUsuarios servicioUsuarios;
        private readonly IMapper mapper;

        public TareasController(ApplicationDbContext context, IServicioUsuarios servicioUsuarios, IMapper mapper)
        {
            this.context = context;
            this.servicioUsuarios = servicioUsuarios;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<List<TareaDTO>> Get()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tareas = await context.Tareas
                .Where(t => t.UsuarioCreacionId == usuarioId)
                .OrderBy(t => t.Orden)
                .ProjectTo<TareaDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            return tareas;
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Tarea>> Get(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();

            var tarea = await context.Tareas.FirstOrDefaultAsync(t => t.UsuarioCreacionId == usuarioId && t.Id == id);

            if(tarea == null)
            {
                return NotFound();
            }

            return tarea;
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] TareaEditarDTO tarea)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tareaEditar = await context.Tareas.FirstOrDefaultAsync(t => t.Id == id && t.UsuarioCreacionId == usuarioId);

            if(tareaEditar == null)
            {
                return NotFound();
            }

            tareaEditar.Titulo = tarea.Titulo;
            tareaEditar.Descripcion = tarea.Descripcion;

            context.SaveChanges();

            return Ok();
        }

    }
}
