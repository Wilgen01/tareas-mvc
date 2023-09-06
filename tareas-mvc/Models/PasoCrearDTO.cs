using System.ComponentModel.DataAnnotations;

namespace tareas_mvc.Models
{
    public class PasoCrearDTO
    {
        [Required]
        public string Descripcion { get; set; }
        public bool Realizado { get; set; }
    }
}
