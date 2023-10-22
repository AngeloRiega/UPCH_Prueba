using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPCH_Prueba.Models;

namespace UPCH_Prueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallesController : ControllerBase
    {
        private readonly DbusuariosContext _context = new DbusuariosContext();

        public DetallesController()
        {

        }

        [ActivatorUtilitiesConstructor]
        public DetallesController(DbusuariosContext context)
        {
            _context = context;
        }

        // GET: api/Detalles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Detalle>>> GetDetalles()
        {
            if (!CheckContextDetalles(_context)) return Problem("Entity set 'DbusuariosContext.Detalles' is null.");

            return Ok(await _context.Detalles.ToListAsync());
        }

        // GET: api/Detalles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Detalle>> GetDetalle(int id)
        {
            if (!CheckContextDetalles(_context)) return Problem("Entity set 'DbusuariosContext.Detalles' is null.");

            var detalle = await _context.Detalles.FindAsync(id);

            if (detalle == null)
            {
                return NotFound($"No se encontró detalle con el DetalleId {id}");
            }

            return detalle;
        }

        // PUT: api/Detalles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetalle(int id, Detalle detalle)
        {
            if (!CheckContextDetalles(_context)) return Problem("Entity set 'DbusuariosContext.Detalles' is null.");

            if (id != detalle.DetalleId)
            {
                return BadRequest($"No coincide el DetalleId. body: {detalle.DetalleId}, path: {id}");
            }

            // Verificar si el usuario existe
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.UserId == detalle.UserId);
            if (user == null)
            {
                return BadRequest("El usuario no existe.");
            }

            _context.Entry(detalle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok("PUT exitoso.");
        }

        // POST: api/Detalles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Detalle>> PostDetalle(Detalle detalle)
        {
            if (!CheckContextDetalles(_context)) return Problem("Entity set 'DbusuariosContext.Detalles' is null.");

            _context.Detalles.Add(detalle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetalle", new { id = detalle.DetalleId }, detalle);
        }

        // DELETE: api/Detalles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalle(int id)
        {
            if (!CheckContextDetalles(_context)) return Problem("Entity set 'DbusuariosContext.Detalles' is null.");

            var detalle = await _context.Detalles.FindAsync(id);
            if (detalle == null)
            {
                return NotFound($"DetalleId {id} no encontrado.");
            }

            _context.Detalles.Remove(detalle);
            await _context.SaveChangesAsync();

            return Ok("DELETE exitoso.");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public bool CheckContextDetalles (DbusuariosContext? context)
        {
            if (context != null)
            {
                if (context.Detalles == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
