using System.ComponentModel.DataAnnotations;

namespace tareas_mvc.Models
{
    public class TareaEditarDTO
    {
        [Required]
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
    }
}
