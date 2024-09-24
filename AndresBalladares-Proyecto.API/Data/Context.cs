using Microsoft.EntityFrameworkCore;
using AndresBalladares_Proyecto.API.Models;

//Context para conectar y utilizar la base de datos con el código.
namespace AndresBalladares_Proyecto.API.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Direccion> Direcciones { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Reporte> Reportes { get; set; }

        //Para conectarse a la base de datos:
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=0118520229;User Id=sa;Password=SQLpa$$2021;");
            }
        }
    }
}