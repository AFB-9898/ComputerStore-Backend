using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TecnicoController : ControllerBase
    {
        private readonly ITecnicoRepository _repo;

        // Constructor - inyección repo
        public TecnicoController(ITecnicoRepository repo) { _repo = repo; }

        // GET api/tecnico - listar todos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _repo.GetAllAsync();
            return Ok(list); // devolver lista
        }

        // GET api/tecnico/{id} - obtener por Id
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var t = await _repo.GetByIdAsync(id);
            if (t == null) return NotFound(); // no existe
            return Ok(t); // devolver objeto
        }

        // POST api/tecnico - crear técnico
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Tecnico entity)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // validar entrada

            entity.UId = Guid.NewGuid(); // generar Id

            try
            {
                await _repo.AddAsync(entity); // guardar
                return CreatedAtAction(nameof(GetById), new { id = entity.UId }, entity); // 201
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error crear técnico", detail = ex.Message }); // error
            }
        }

        // PUT api/tecnico/{id} - actualizar
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Tecnico entity)
        {
            if (id != entity.UId) return BadRequest("Id no coincide"); // validar Id

            try
            {
                await _repo.UpdateAsync(entity); // actualizar
                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error actualizar técnico", detail = ex.Message }); // error
            }
        }

        // DELETE api/tecnico/{id} - eliminar
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _repo.DeleteAsync(id); // eliminar
                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error eliminar técnico", detail = ex.Message }); // error
            }
        }
    }
}
