using Microsoft.EntityFrameworkCore;

namespace Agroservicio
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        //Constructor del contexto de base de datos

        public DbContext(DbContextOptions<DbContext> options ): base (options)
        {
        }

        public DbSet<Models.Marca> Marca { get; set; }
        public DbSet<Models.Usuario> Usuario { get; set; }

    }
}
