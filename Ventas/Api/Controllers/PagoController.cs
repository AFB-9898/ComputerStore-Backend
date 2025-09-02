using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Interfaces;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly IPagoRepository _repo;
        public PagoController(IPagoRepository repo) { _repo = repo; }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _repo.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        [HttpGet("orden/{ordenId:guid}")]
        public async Task<IActionResult> GetByOrden(Guid ordenId) =>
            Ok(await _repo.GetByOrdenIdAsync(ordenId));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Pago pago)
        {
            await _repo.AddAsync(pago);
            return CreatedAtAction(nameof(GetById), new { id = pago.UId }, pago);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Pago pago)
        {
            if (id != pago.UId) return BadRequest("Id no coincide");
            await _repo.UpdateAsync(pago);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
