using System.Linq;
using Curso.Data.Configurations;
using Curso.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Curso.Data
{
    public class ApplicationContext : DbContext
    {
        private static readonly ILoggerFactory _logger = LoggerFactory.Create(p => p.AddConsole());
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(_logger)
                .EnableSensitiveDataLogging()
                .UseSqlServer("Data source=(localdb)\\mssqllocaldb; Initial Catalog=Curso EF Core;Integrated Security=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //fazer uma por uma
            //modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            //modelBuilder.ApplyConfiguration(new PedidoConfiguration());
            //modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
            //modelBuilder.ApplyConfiguration(new PedidoItemConfiguration());

            //fazer para todos
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
            MapearPropriedadesEsquecidas(modelBuilder);
        }

        private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entity.GetProperties().Where(p => p.ClrType == typeof(string));

                foreach (var property in properties)
                {
                    if (string.IsNullOrEmpty(property.GetColumnType())
                        && !property.GetMaxLength().HasValue)
                    {
                        //property.SetMaxLength(100);
                        property.SetColumnType("VARCHAR(100)");
                    }
                }
            }
        }
    }
}