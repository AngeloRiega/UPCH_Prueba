using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPCH_Prueba.Controllers;
using UPCH_Prueba.Models;

namespace UPCH_Prueba_Tests
{
    public class UsuariosControllerTests : TestBase
    {
        [Fact]
        public async Task GetUsuarios_ReturnsListaUsuarios()
        {
            // Arrange
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
        public async Task GetUsuario_ReturnsUsuario()
        {
            // Arrange
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
        public async Task GetUsuario_NotFoundNoExisteUsuario()
        {
            // Arrange
            using (var context = GetTestDbContext())
            {
                var controller = new UsuariosController(context);

                // Act
                var result = await controller.GetUsuario(999);

                // Assert
                Assert.IsType<NotFoundObjectResult>(result.Result!);
            }
        }

        [Fact]
        public async Task GetUsuarioDetalle_ReturnsUsuarioDetalles()
        {
            // Arrange
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
            // Arrange
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
            // Arrange
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
        public async Task PutUsuario_UpdatesUsuario()
        {
            // Arrange
            using (var context = GetTestDbContext())
            {
                var controller = new UsuariosController(context);
                var updatedUsuario = new Usuario { UserId = 1, Nombre = "NuevoNombre", Apellido = "NuevoApellido", Email = "nuevo.email@gmail.com", FechaRegistro = new DateTime(), Activo = false };

                // Act
                var result = await controller.PutUsuario(1, updatedUsuario);

                // Assert
                Assert.IsType<OkObjectResult>(result);
                var resultUsuario = context.Usuarios.Find(1);
                Assert.Equal("NuevoNombre", resultUsuario!.Nombre);
                Assert.Equal("NuevoApellido", resultUsuario.Apellido);
            }
        }

        [Fact]
        public async Task PutUsuario_BadRequestDiferenteId() // Se esta haciendo update a un Id diferente.
        {
            // Arrange
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
        public async Task PostUsuario_CreatesUsuario()
        {
            // Arrange
            using (var context = GetTestDbContext())
            {
                var controller = new UsuariosController(context);
                var newUsuario = new Usuario { Nombre = "NuevoUsuario", Apellido = "ApellidoNuevo", Email = "nuevo.usuario@gmail.com", FechaRegistro = new DateTime(), Activo = true };

                // Act
                var result = await controller.PostUsuario(newUsuario);

                // Assert
                var actionResult = Assert.IsType<ActionResult<Usuario>>(result);
                var createdUsuario = Assert.IsAssignableFrom<Usuario>(((ObjectResult)actionResult.Result!).Value);
                Assert.Equal(newUsuario.Nombre, createdUsuario.Nombre);
                Assert.IsType<int>(createdUsuario.UserId);
            }
        }

        [Fact]
        public async Task DeleteUsuario_DeletesUsuario()
        {
            // Arrange
            using (var context = GetTestDbContext())
            {
                var controller = new UsuariosController(context);

                // Act
                var result = await controller.DeleteUsuario(2);

                // Assert
                Assert.IsType<OkObjectResult>(result);
                var deletedUsuario = context.Usuarios.Find(2);
                Assert.Null(deletedUsuario);
            }
        }

        [Fact]
        public async Task DeleteUsuario_NotFoundNoExisteUsuario()
        {
            // Arrange
            using (var context = GetTestDbContext())
            {
                var controller = new UsuariosController(context);

                // Act
                var result = await controller.DeleteUsuario(999);

                // Assert
                Assert.IsType<NotFoundObjectResult>(result);
            }
        }

        [Fact]
        public async Task DeleteUsuario_BadRequestTieneDetalles() // El usuario tiene detalles relacionados.
        {
            // Arrange
            using (var context = GetTestDbContext())
            {
                var controller = new UsuariosController(context);

                // Act
                var result = await controller.DeleteUsuario(1);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }

        [Fact]
        public void CheckContextUsuarios_ContextNotNullAndDetallesNotNull_ReturnsTrue()
        {
            // Arrange
            var controller = new UsuariosController();

            // Act
            bool result = controller.CheckContextUsuarios(new DbusuariosContext());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckContextUsuarios_ContextNotNullAndDetallesNull_ReturnsFalse()
        {
            // Arrange
            var controller = new UsuariosController();

            // Act
            bool result = controller.CheckContextUsuarios(new DbusuariosContext { Usuarios = null! });

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CheckContextUsuarios_ContextNull_ReturnsFalse()
        {
            // Arrange
            var controller = new UsuariosController();

            // Act
            bool result = controller.CheckContextUsuarios(null);

            // Assert
            Assert.False(result);
        }
    }
}
