using Microsoft.EntityFrameworkCore;

namespace tareas_mvc
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
