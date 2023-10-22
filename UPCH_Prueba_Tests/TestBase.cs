using System;
using Microsoft.EntityFrameworkCore;
using UPCH_Prueba.Models;

namespace UPCH_Prueba_Tests
{
    public class TestBase
    {
        protected static DbusuariosContext GetTestDbContext()
        {
            // Configura una base de datos en memoria para las pruebas.
            var options = new DbContextOptionsBuilder<DbusuariosContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            using (var context = new DbusuariosContext(options))
            {
                var usuario1 = new Usuario
                {
                    UserId = 1,
                    Nombre = "Juan",
                    Apellido = "Pérez",
                    Email = "juan.perez@gmail.com",
                    FechaRegistro = new DateTime(),
                    Activo = true
                };
                context.Usuarios.Add(usuario1);

                var usuario2 = new Usuario
                {
                    UserId = 2,
                    Nombre = "María",
                    Apellido = "Gómez",
                    Email = "maria.gomez@gmail.com",
                    FechaRegistro = new DateTime(),
                    Activo = false
                };
                context.Usuarios.Add(usuario2);

                var detalle1 = new Detalle
                {
                    DetalleId = 1,
                    UserId = 1,
                    Telefono = "123456789",
                    Direccion = "Calle 123",
                    Ciudad = "Ciudad",
                    Provincia = "Provincia",
                    CodigoPostal = "12345"
                };
                context.Detalles.Add(detalle1);

                context.SaveChanges();
            }

            return new DbusuariosContext(options);
        }
    }
}