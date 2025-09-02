    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Domain.Interfaces;
    using Microsoft.EntityFrameworkCore;

    namespace Api.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class InventarioController : ControllerBase
        {
            private readonly IInventarioRepository _repo;

            public InventarioController(IInventarioRepository repo)
            {
                _repo = repo;
            }

            // GET api/inventario
            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var inventarios = await _repo.GetAllAsync();
                return Ok(inventarios);
            }

            // GET api/inventario/product/{productId}
            [HttpGet("product/{productId:guid}")]
            public async Task<IActionResult> GetByProduct(Guid productId)
            {
                var inventario = await _repo.GetByProductIdAsync(productId);
                if (inventario == null) return NotFound();
                return Ok(inventario);
            }

            // DTO para ajuste de inventario
            public class AjusteInventarioDto
            {
                /// <summary>
                /// Delta de cantidad: positivo para ingreso, negativo para salida/venta.
                /// </summary>
                public int CantidadDelta { get; set; }
                public string? Motivo { get; set; }
            }

            // PUT api/inventario/product/{productId}/ajustar
            [HttpPut("product/{productId:guid}/ajustar")]
            public async Task<IActionResult> Ajustar(Guid productId, [FromBody] AjusteInventarioDto dto)
            {
                if (dto == null) return BadRequest("Payload inválido");
                if (dto.CantidadDelta == 0) return BadRequest("CantidadDelta no puede ser 0");

                try
                {
                    await _repo.AdjustStockAsync(productId, dto.CantidadDelta);
                    return NoContent();
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "Error al ajustar inventario", detail = ex.Message });
                }
            }

            // PUT api/inventario/{id}
            [HttpPut("{id:guid}")]
            public async Task<IActionResult> UpdateById(Guid id, [FromBody] Inventario ajuste)
            {
                if (ajuste == null) return BadRequest();
                if (id != ajuste.UId) return BadRequest("Id no coincide");

                try
                {
                    await _repo.UpdateAsync(ajuste);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "Error al actualizar inventario", detail = ex.Message });
                }
            }

            // DELETE api/inventario/{id}
            [HttpDelete("{id:guid}")]
            public async Task<IActionResult> DeleteById(Guid id)
            {
                try
                {
                    await _repo.DeleteAsync(id);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "Error al eliminar inventario", detail = ex.Message });
                }
            }
        }
    }
