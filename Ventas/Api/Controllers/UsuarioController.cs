using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _repo;

        // Constructor - inyección repo
        public UsuarioController(IUsuarioRepository repo) { _repo = repo; }

        // GET api/usuario - listar todos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _repo.GetAllAsync();
            return Ok(list); // devolver lista
        }

        // GET api/usuario/{id} - obtener por Id
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var u = await _repo.GetByIdAsync(id);
            if (u == null) return NotFound(); // no existe
            return Ok(u); // devolver objeto
        }

        // POST api/usuario - crear usuario
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Usuario user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // validar entrada

            user.UId = Guid.NewGuid(); // generar Id

            try
            {
                await _repo.AddAsync(user); // guardar
                return CreatedAtAction(nameof(GetById), new { id = user.UId }, user); // 201
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error crear usuario", detail = ex.Message }); // error
            }
        }

        // PUT api/usuario/{id} - actualizar
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Usuario user)
        {
            if (id != user.UId) return BadRequest("Id no coincide"); // validar Id

            try
            {
                await _repo.UpdateAsync(user); // actualizar
                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error actualizar usuario", detail = ex.Message }); // error
            }
        }

        // DELETE api/usuario/{id} - eliminar
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
                return StatusCode(500, new { message = "Error eliminar usuario", detail = ex.Message }); // error
            }
        }
    }
}
