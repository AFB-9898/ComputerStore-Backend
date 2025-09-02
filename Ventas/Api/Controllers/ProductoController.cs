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
    public class ProductoController : ControllerBase
    {
        private readonly IProductoRepository _productoRepository;
        private readonly ICategoriaRepository _categoriaRepository;

        public ProductoController(IProductoRepository productoRepository, ICategoriaRepository categoriaRepository)
        {
            _productoRepository = productoRepository;
            _categoriaRepository = categoriaRepository;
        }

        // GET api/producto
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productos = await _productoRepository.GetAllAsync();
            return Ok(productos);
        }

        // GET api/producto/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var producto = await _productoRepository.GetByIdAsync(id);
            if (producto == null)
                return NotFound(new { message = "Producto no encontrado" });

            return Ok(producto);
        }

        // POST api/producto
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Producto producto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Datos inválidos", errors = ModelState });

            try
            {
                // Si no se especifica categoría, asigna una por defecto (opcional)
                if (producto.CategoriaId == Guid.Empty)
                {
                    // Ajusta este valor si tienes una categoría default
                    var defaultCategoria = await _categoriaRepository.GetAllAsync();
                    if (defaultCategoria == null)
                        return BadRequest("Debe especificar una categoría válida.");

                    producto.CategoriaId = defaultCategoria.First().UId;
                }
                else
                {
                    var categoria = await _categoriaRepository.GetByIdAsync(producto.CategoriaId);
                    if (categoria == null)
                        return BadRequest($"No existe la categoría {producto.CategoriaId}");
                }

                producto.UId = Guid.NewGuid();
                await _productoRepository.AddAsync(producto);

                return CreatedAtAction(nameof(GetById), new { id = producto.UId }, producto);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { message = "Error al guardar en la base de datos", detail = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado al crear producto", detail = ex.Message });
            }
        }

        // PUT api/producto/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Producto producto)
        {
            if (id != producto.UId)
                return BadRequest(new { message = "El ID del producto no coincide" });

            try
            {
                // Validar si el producto existe
                var existente = await _productoRepository.GetByIdAsync(id);
                if (existente == null)
                    return NotFound(new { message = "Producto no encontrado" });

                // Si se envió categoría, validar
                if (producto.CategoriaId != Guid.Empty)
                {
                    var categoria = await _categoriaRepository.GetByIdAsync(producto.CategoriaId);
                    if (categoria == null)
                        return BadRequest(new { message = $"No existe la categoría {producto.CategoriaId}" });
                }

                await _productoRepository.UpdateAsync(producto);
                return NoContent();
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { message = "Error al actualizar en DB", detail = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado al actualizar producto", detail = ex.Message });
            }
        }

        // DELETE api/producto/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var existente = await _productoRepository.GetByIdAsync(id);
                if (existente == null)
                    return NotFound(new { message = "Producto no encontrado" });

                await _productoRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado al eliminar producto", detail = ex.Message });
            }
        }
    }
}
    