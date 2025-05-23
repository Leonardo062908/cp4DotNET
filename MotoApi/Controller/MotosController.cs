using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoApi.Data;
using MotoApi.Models;

namespace MotoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MotosController(AppDbContext context) => _context = context;

        // GET: api/motos?statusMotoId=1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Moto>>> GetMotos([FromQuery] int? statusMotoId)
        {
            var query = _context.Motos.AsQueryable();
            if (statusMotoId.HasValue)
                query = query.Where(m => m.StatusMotoId == statusMotoId.Value);
            return Ok(await query.ToListAsync());
        }

        // GET: api/motos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Moto>> GetMoto(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null) return NotFound();
            return Ok(moto);
        }

        // GET: api/motos/search?modelo=
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Moto>>> SearchMotos([FromQuery] string modelo)
        {
            if (string.IsNullOrWhiteSpace(modelo))
                return BadRequest("Parâmetro 'modelo' obrigatório.");
            var result = await _context.Motos
                                       .Where(m => m.Modelo.Contains(modelo))
                                       .ToListAsync();
            return Ok(result);
        }

        // POST: api/motos
        [HttpPost]
        public async Task<ActionResult<Moto>> CreateMoto(Moto moto)
        {
            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMoto), new { id = moto.IdMoto }, moto);
        }

        // PUT: api/motos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMoto(int id, Moto moto)
        {
            if (id != moto.IdMoto)
                return BadRequest("ID de rota diferente do objeto.");

            _context.Entry(moto).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Motos.AnyAsync(e => e.IdMoto == id))
                    return NotFound();
                throw;
            }
            return NoContent();
        }

        // DELETE: api/motos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoto(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null) return NotFound();
            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
