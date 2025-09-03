using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoApi.Data;
using MotoApi.Models;
using MotoApi.Dtos;

namespace MotoApi.Controllers;

[ApiController]
[Route("api/motos")]
public class MotosController : ControllerBase
{
    private readonly AppDbContext _context;
    public MotosController(AppDbContext context) => _context = context;

    // GET: api/motos?statusMotoId=1
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MotoResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] int? statusMotoId, CancellationToken ct)
    {
        var q = _context.Motos.AsNoTracking().AsQueryable();

        if (statusMotoId.HasValue)
            q = q.Where(m => m.StatusMotoId == statusMotoId.Value);

        var list = await q
            .Select(m => new MotoResponseDto(m.IdMoto, m.Modelo, m.Placa, m.StatusMotoId))
            .ToListAsync(ct);

        return Ok(list);
    }

    // GET: api/motos/{id}
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MotoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken ct)
    {
        var m = await _context.Motos.AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.IdMoto == id, ct);
        if (m is null) return NotFound();

        return Ok(new MotoResponseDto(m.IdMoto, m.Modelo, m.Placa, m.StatusMotoId));
    }

    // GET: api/motos/search?modelo=FAN
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<MotoResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] string? modelo, CancellationToken ct)
    {
        var q = _context.Motos.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(modelo))
        {
            var term = modelo.Trim().ToUpperInvariant();
            q = q.Where(m => m.Modelo.ToUpper()!.Contains(term));
        }

        var list = await q.Select(m => new MotoResponseDto(m.IdMoto, m.Modelo, m.Placa, m.StatusMotoId))
                          .ToListAsync(ct);
        return Ok(list);
    }

    // POST: api/motos
    [HttpPost]
    [ProducesResponseType(typeof(MotoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] MotoCreateDto dto, CancellationToken ct)
    {
        // [ApiController] já faz ModelState validation; chega aqui se válido
        var m = new Moto
        {
            Modelo = dto.Modelo.Trim().ToUpperInvariant(),
            Placa = dto.Placa.Trim().ToUpperInvariant(),
            StatusMotoId = dto.StatusMotoId
        };

        _context.Motos.Add(m);
        await _context.SaveChangesAsync(ct);

        var resp = new MotoResponseDto(m.IdMoto, m.Modelo, m.Placa, m.StatusMotoId);
        return CreatedAtAction(nameof(GetById), new { id = m.IdMoto }, resp);
    }

    // PUT: api/motos/{id}
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] MotoUpdateDto dto, CancellationToken ct)
    {
        var m = await _context.Motos.FirstOrDefaultAsync(x => x.IdMoto == id, ct);
        if (m is null) return NotFound();

        m.Modelo = dto.Modelo.Trim().ToUpperInvariant();
        m.Placa = dto.Placa.Trim().ToUpperInvariant();
        m.StatusMotoId = dto.StatusMotoId;

        await _context.SaveChangesAsync(ct);
        return NoContent();
    }

    // DELETE: api/motos/{id}
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
    {
        var m = await _context.Motos.FindAsync([id], ct);
        if (m is null) return NotFound();

        _context.Motos.Remove(m);
        await _context.SaveChangesAsync(ct);
        return NoContent();
    }
}
