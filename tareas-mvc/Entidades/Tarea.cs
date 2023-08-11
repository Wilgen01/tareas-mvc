using System.ComponentModel.DataAnnotations;

namespace tareas_mvc.Entidades
{
    public class Tarea
    {
        public int Id { get; set; }
        [StringLength(250)]
        [Required]
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Orden { get; set;}
        public DateTime FechaCreacion { get; set; }

    }
}
