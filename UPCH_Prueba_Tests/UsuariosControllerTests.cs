using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPCH_Prueba.Controllers;
using UPCH_Prueba.Models;

namespace UPCH_Prueba.Tests
{
    public class UsuariosControllerTests
    {
        private DbusuariosContext GetTestDbContext()
        {
            // Configura una base de datos en memoria para las pruebas.
            var options = new DbContextOptionsBuilder<DbusuariosContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
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


        [Fact]
        public async Task GetUsuarios_ReturnsListOfUsers()
        {
            using (var context = GetTestDbContext())
            {
                var controller = new UsuariosController(context);

                // Act
                var result = await controller.GetUsuarios();

                // Assert
                var actionResult = Assert.IsType<ActionResult<IEnumerable<Usuario>>>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Usuario>>(((ObjectResult)actionResult.Result!).Value);
                Assert.Equal(2, model.Count());
            }
        }

        [Fact]
        public async Task GetUsuario_ReturnsUser()
        {
            using (var context = GetTestDbContext())
            {
                var controller = new UsuariosController(context);

                // Act
                var result = await controller.GetUsuario(1);

                // Assert
                var actionResult = Assert.IsType<ActionResult<Usuario>>(result);
                var model = Assert.IsAssignableFrom<Usuario>(((ObjectResult)actionResult.Result!).Value);
                Assert.Equal("Juan", model.Nombre);
                Assert.Equal("Pérez", model.Apellido);
            }
        }

        [Fact]
        public async Task GetUsuarioDetalle_ReturnsUserDetails()
        {
            using (var context = GetTestDbContext())
            {
                var controller = new UsuariosController(context);

                // Act
                var result = await controller.GetUsuarioDetalle(1);

                // Assert
                var actionResult = Assert.IsType<ActionResult<IEnumerable<Detalle>>>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Detalle>>(((ObjectResult)actionResult.Result!).Value);
                Assert.Single(model); // Debe haber un detalle
                Assert.Equal("123456789", model.First().Telefono);
                Assert.Equal("Calle 123", model.First().Direccion);
            }
        }

        [Fact]
        public async Task GetUsuarioDetalle_NotFoundNoExisteDetalle()
        {
            using (var context = GetTestDbContext())
            {
                var controller = new UsuariosController(context);

                // Act
                var result = await controller.GetUsuarioDetalle(2);

                // Assert
                Assert.IsType<NotFoundObjectResult>(result.Result!);
            }
        }

        [Fact]
        public async Task GetUsuarioDetalle_NotFoundNoExisteUsuario()
        {
            using (var context = GetTestDbContext())
            {
                var controller = new UsuariosController(context);

                // Act
                var result = await controller.GetUsuarioDetalle(123);

                // Assert
                Assert.IsType<NotFoundObjectResult>(result.Result!);
            }
        }

        [Fact]
        public async Task PutUsuario_UpdatesUser()
        {
            using (var context = GetTestDbContext())
            {
                var controller = new UsuariosController(context);
                var updatedUsuario = new Usuario { UserId = 1, Nombre = "NuevoNombre", Apellido = "NuevoApellido", Email = "nuevo.email@gmail.com", FechaRegistro = new DateTime(), Activo = false };

                // Act
                var result = await controller.PutUsuario(1, updatedUsuario);

                // Assert
                Assert.IsType<OkObjectResult>(result);
                var updatedUser = context.Usuarios.Find(1);
                Assert.Equal("NuevoNombre", updatedUser!.Nombre);
                Assert.Equal("NuevoApellido", updatedUser.Apellido);
            }
        }

        [Fact]
        public async Task PutUsuario_BadRequestDiferenteId() // Se esta haciendo update al Id.
        {
            using (var context = GetTestDbContext())
            {
                var controller = new UsuariosController(context);
                var updatedUsuario = new Usuario { UserId = 3, Nombre = "NuevoNombre", Apellido = "NuevoApellido", Email = "nuevo.email@gmail.com", FechaRegistro = new DateTime(), Activo = false };

                // Act
                var result = await controller.PutUsuario(1, updatedUsuario);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }


        [Fact]
        public async Task PostUsuario_CreatesUser()
        {
            using (var context = GetTestDbContext())
            {
                var controller = new UsuariosController(context);
                var newUser = new Usuario { Nombre = "NuevoUsuario", Apellido = "ApellidoNuevo", Email = "nuevo.usuario@gmail.com", FechaRegistro = new DateTime(), Activo = true };

                // Act
                var result = await controller.PostUsuario(newUser);

                // Assert
                var actionResult = Assert.IsType<ActionResult<Usuario>>(result);
                var createdUser = Assert.IsAssignableFrom<Usuario>(((ObjectResult)actionResult.Result!).Value);
                Assert.Equal(newUser.Nombre, createdUser.Nombre);
            }
        }

        [Fact]
        public async Task DeleteUsuario_DeletesUser()
        {
            using (var context = GetTestDbContext())
            {
                var controller = new UsuariosController(context);

                // Act
                var result = await controller.DeleteUsuario(2);

                // Assert
                Assert.IsType<OkObjectResult>(result);
                var deletedUser = context.Usuarios.Find(2);
                Assert.Null(deletedUser);
            }
        }

        [Fact]
        public async Task DeleteUsuario_BadRequestTieneDetalles() // El usuario tiene detalles relacionados.
        {
            using (var context = GetTestDbContext())
            {
                var controller = new UsuariosController(context);

                // Act
                var result = await controller.DeleteUsuario(1);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }
    }
}
