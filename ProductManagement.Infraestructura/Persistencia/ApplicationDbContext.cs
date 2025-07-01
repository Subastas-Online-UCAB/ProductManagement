using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Dominio.Entidades;

namespace UsuarioServicio.Infraestructura.Persistencia
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Producto> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Opcional: Cambiar nombre de la tabla si quieres
            modelBuilder.Entity<Producto>().ToTable("Productos");

            // Opcional: Configuraciones de columnas si quieres afinar
            modelBuilder.Entity<Producto>(entity =>
            {
              

                entity.HasKey(s => s.IdProducto);

                entity.Property(s => s.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(s => s.Descripcion)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(s => s.Tipo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(s => s.Cantidad)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");


                entity.Property(s => s.IdUsuario)
                    .IsRequired();

            });
        }
    }
}

