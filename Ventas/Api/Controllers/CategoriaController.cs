using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepository _repo;

        // Constructor - inyección de repo
        public CategoriaController(ICategoriaRepository repo)
        {
            _repo = repo;
        }

        // GET api/categoria - obtener todas las categorías
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categorias = await _repo.GetAllAsync();
            return Ok(categorias); // OK + lista
        }

        // GET api/categoria/{id} - obtener categoría por id
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var categoria = await _repo.GetByIdAsync(id);
            if (categoria == null)
                return NotFound(); // no existe
            return Ok(categoria); // devolver objeto
        }

        // POST api/categoria - crear nueva categoría
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Categoria categoria)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // validar entrada

            try
            {
                categoria.UId = Guid.NewGuid(); // generar Id backend
                await _repo.AddAsync(categoria); // guardar
                return CreatedAtAction(nameof(GetById), new { id = categoria.UId }, categoria); // devolver 201
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear categoría", detail = ex.Message }); // error
            }
        }

        // PUT api/categoria/{id} - actualizar categoría
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Categoria categoria)
        {
            if (id != categoria.UId)
                return BadRequest("El UId no coincide"); // validar Id

            try
            {
                await _repo.UpdateAsync(categoria); // actualizar
                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar categoría", detail = ex.Message }); // error
            }
        }

        // DELETE api/categoria/{id} - eliminar categoría
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
                return StatusCode(500, new { message = "Error al eliminar categoría", detail = ex.Message }); // error
            }
        }
    }
}
