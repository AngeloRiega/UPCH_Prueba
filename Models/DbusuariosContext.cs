using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UPCH_Prueba.Models;

public partial class DbusuariosContext : DbContext
{
    public DbusuariosContext()
    {
    }

    public DbusuariosContext(DbContextOptions<DbusuariosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Detalle> Detalles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //Busco el connection string desde appsettings.json
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .Build();

            var connectionString = configuration.GetConnectionString("UPCHCS");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                //Llenar connectionString en el appsettings.json
                throw new ArgumentNullException("sqlserver connectionString must not be null (Llenar connectionString en el appsettings.json)", connectionString);
            }

            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Detalle>(entity =>
        {
            entity.HasKey(e => e.DetalleId).HasName("PK__Detalles__6E19D6FA174206B4");

            entity.Property(e => e.DetalleId).HasColumnName("DetalleID");
            entity.Property(e => e.Ciudad).HasMaxLength(50);
            entity.Property(e => e.CodigoPostal).HasMaxLength(10);
            entity.Property(e => e.Direccion).HasMaxLength(100);
            entity.Property(e => e.Provincia).HasMaxLength(50);
            entity.Property(e => e.Telefono).HasMaxLength(20);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Detalles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Detalles_Usuarios");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Usuarios__1788CCAC6C3C74B7");

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D10534B95F5923").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Apellido).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
