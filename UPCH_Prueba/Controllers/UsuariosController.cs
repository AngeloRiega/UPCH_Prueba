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
    public class UsuariosController : ControllerBase
    {
        private readonly DbusuariosContext _context;

        public UsuariosController(DbusuariosContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
          if (_context.Usuarios == null)
          {
              return NotFound();
          }
            return Ok(await _context.Usuarios.ToListAsync());
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
          if (_context.Usuarios == null)
          {
              return NotFound();
          }
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound($"No se encontró el UserId {id}");
            }

            return Ok(usuario);
        }

        // GET: api/Usuarios/5/detalles
        [HttpGet("{id}/detalles")]
        public async Task<ActionResult<IEnumerable<Detalle>>> GetUsuarioDetalle(int id)
        {
            if (_context.Usuarios == null)
            {
                return NotFound();
            }
            
            var usuario = await _context.Usuarios.Include(u => u.Detalles).FirstOrDefaultAsync(u => u.UserId == id);

            if (usuario == null)
            {
                return NotFound($"No se encontró el UserId {id}");
            }

            if (!usuario.Detalles.Any())
            {
                return NotFound($"No se encontraron detalles con el UserId {id}");
            }

            return Ok(usuario.Detalles);
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.UserId)
            {
                return BadRequest($"No coincide el UserId. body: {usuario.UserId}, path: {id}");
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("PUT exitoso.");
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
          if (_context.Usuarios == null)
          {
              return Problem("Entity set 'DbusuariosContext.Usuarios'  is null.");
          }
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.UserId }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            if (_context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound($"UserId {id} no encontrado.");
            }

            // Verificar si existen detalles relacionados
            var detalles = await _context.Detalles.Where(d => d.UserId == id).ToListAsync();
            if (detalles.Any())
            {
                var detalleIds = string.Join(", ", detalles.Select(d => d.DetalleId));
                return BadRequest($"El usuario tiene detalles relacionados y no puede ser eliminado. DetalleId: {detalleIds}");
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok("DELETE exitoso.");
        }

        private bool UsuarioExists(int id)
        {
            return (_context.Usuarios?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
