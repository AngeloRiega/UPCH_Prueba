using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPCH_Prueba.Controllers;
using UPCH_Prueba.Models;

namespace UPCH_Prueba_Tests
{
    public class DetallesControllerTests : TestBase
    {
        [Fact]
        public async Task GetDetalles_ReturnsListaDetalles()
        {
            // Arrange
            using (var context = GetTestDbContext())
            {
                var controller = new DetallesController(context);

                // Act
                var result = await controller.GetDetalles();

                // Assert
                var actionResult = Assert.IsType<ActionResult<IEnumerable<Detalle>>>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Detalle>>(((ObjectResult)actionResult.Result!).Value);
                Assert.Single(model);
            }
        }

        [Fact]
        public async Task GetDetalle_ReturnsDetalle()
        {
            // Arrange
            using (var context = GetTestDbContext())
            {
                var controller = new DetallesController(context);

                // Act
                var result = await controller.GetDetalle(1);

                // Assert
                var actionResult = Assert.IsType<ActionResult<Detalle>>(result);
                var model = Assert.IsAssignableFrom<Detalle>(actionResult.Value);
                Assert.Equal("123456789", model.Telefono);
                Assert.Equal("Calle 123", model.Direccion);
            }
        }

        [Fact]
        public async Task GetDetalle_NotFoundNoExisteDetalle()
        {
            // Arrange
            using (var context = GetTestDbContext())
            {
                var controller = new DetallesController(context);

                // Act
                var result = await controller.GetDetalle(999);

                // Assert
                Assert.IsType<NotFoundObjectResult>(result.Result!);
            }
        }

        [Fact]
        public async Task PutDetalle_UpdatesDetalle()
        {
            // Arrange
            using (var context = GetTestDbContext())
            {
                var controller = new DetallesController(context);
                var updatedDetalle = new Detalle { DetalleId = 1, UserId = 2, Telefono = "987654321", Direccion = "Calle 456" };

                // Act
                var result = await controller.PutDetalle(1, updatedDetalle);

                // Assert
                Assert.IsType<OkObjectResult>(result);
                var resultDetalle = context.Detalles.Find(1);
                Assert.Equal(updatedDetalle.Telefono, resultDetalle!.Telefono);
                Assert.Equal(updatedDetalle.Direccion, resultDetalle.Direccion);
            }
        }

        [Fact]
        public async Task PutDetalle_BadRequestDiferenteId() // Se esta haciendo update a un Id diferente.
        {
            // Arrange
            using (var context = GetTestDbContext())
            {
                var controller = new DetallesController(context);
                var updatedDetalle = new Detalle { DetalleId = 5, Telefono = "987654321", Direccion = "Calle 456" };

                // Act
                var result = await controller.PutDetalle(1, updatedDetalle);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }

        [Fact]
        public async Task PutDetalle_BadRequestNoExisteUsuario()
        {
            // Arrange
            using (var context = GetTestDbContext())
            {
                var controller = new DetallesController(context);
                var updatedDetalle = new Detalle { DetalleId = 1, UserId = 999};

                // Act
                var result = await controller.PutDetalle(1, updatedDetalle);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }

        [Fact]
        public async Task PostDetalle_CreatesDetalle()
        {
            // Arrange
            using (var context = GetTestDbContext())
            {
                var controller = new DetallesController(context);
                var newDetalle = new Detalle { UserId = 1, Telefono = "456123789", Ciudad = "Ciudad2", Provincia = "Provincia2", Direccion = "Avenida", CodigoPostal = "55555" };

                // Act
                var result = await controller.PostDetalle(newDetalle);

                // Assert
                var actionResult = Assert.IsType<ActionResult<Detalle>>(result);
                var createdDetalle = Assert.IsAssignableFrom<Detalle>(((ObjectResult)actionResult.Result!).Value);
                Assert.Equal(newDetalle.Telefono, createdDetalle.Telefono);
                Assert.IsType<int>(createdDetalle.DetalleId);
            }
        }

        [Fact]
        public async Task DeleteDetalle_DeletesDetalle()
        {
            // Arrange
            using (var context = GetTestDbContext())
            {
                var controller = new DetallesController(context);

                // Act
                var result = await controller.DeleteDetalle(1);

                // Assert
                Assert.IsType<OkObjectResult>(result);
                var deletedDetalle = context.Detalles.Find(1);
                Assert.Null(deletedDetalle);
            }
        }

        [Fact]
        public async Task DeleteDetalle_BadRequestNoExisteDetalle()
        {
            // Arrange
            using (var context = GetTestDbContext())
            {
                var controller = new DetallesController(context);

                // Act
                var result = await controller.DeleteDetalle(50);

                // Assert
                Assert.IsType<NotFoundObjectResult>(result);
            }
        }

        [Fact]
        public void CheckContextDetalles_ContextNotNullAndDetallesNotNull_ReturnsTrue()
        {
            // Arrange
            var controller = new DetallesController();

            // Act
            bool result = controller.CheckContextDetalles(new DbusuariosContext());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckContextDetalles_ContextNotNullAndDetallesNull_ReturnsFalse()
        {
            // Arrange
            var controller = new DetallesController();

            // Act
            bool result = controller.CheckContextDetalles(new DbusuariosContext { Detalles = null! });

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CheckContextDetalles_ContextNull_ReturnsFalse()
        {
            // Arrange
            var controller = new DetallesController();

            // Act
            bool result = controller.CheckContextDetalles(null);

            // Assert
            Assert.False(result);
        }
    }
}
