using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Interfaces;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarritoController : ControllerBase
    {
        private readonly ICarritoRepository _carritoRepo;
        private readonly IProductoRepository _productoRepo;

        public CarritoController(ICarritoRepository carritoRepo, IProductoRepository productoRepo)
        {
            _carritoRepo = carritoRepo;
            _productoRepo = productoRepo;
        }

        // DTO para agregar item
        public class AddCarritoItemDto
        {
            public Guid UsuarioId { get; set; }
            public Guid ProductoId { get; set; }
            public int Cantidad { get; set; } = 1;
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem([FromBody] AddCarritoItemDto dto)
        {
            if (dto == null) return BadRequest("Datos inválidos");
            if (dto.Cantidad <= 0) return BadRequest("Cantidad debe ser mayor que 0");

            // validar producto
            var producto = await _productoRepo.GetByIdAsync(dto.ProductoId);
            if (producto == null) return BadRequest("Producto no encontrado");
            if (producto.StockActual < dto.Cantidad) return BadRequest("Stock insuficiente");

            try
            {
                // Obtener carrito del usuario (primer carrito si hay varios)
                var carritosUsuario = await _carritoRepo.GetByUsuarioAsync(dto.UsuarioId);
                var carrito = carritosUsuario?.FirstOrDefault();

                if (carrito == null)
                {
                    // Crear nuevo carrito y añadir item
                    carrito = new Carrito
                    {
                        UsuarioId = dto.UsuarioId,
                        FechaCreacion = DateTime.UtcNow
                    };

                    var nuevoItem = new CarritoItem
                    {
                        UId = Guid.NewGuid(),
                        CarritoId = carrito.UId,       // FK al carrito
                        ProductoId = dto.ProductoId,
                        Cantidad = dto.Cantidad
                    };

                    carrito.Items.Add(nuevoItem);

                    await _carritoRepo.AddAsync(carrito);
                    return Ok(carrito);
                }
                else
                {
                    // Buscar si existe el producto en el carrito (por ProductoId)
                    var existente = carrito.Items.FirstOrDefault(i => i.ProductoId == dto.ProductoId);
                    if (existente != null)
                    {
                        existente.Cantidad += dto.Cantidad;
                    }
                    else
                    {
                        var nuevoItem = new CarritoItem
                        {
                            UId = Guid.NewGuid(),
                            CarritoId = carrito.UId,
                            ProductoId = dto.ProductoId,
                            Cantidad = dto.Cantidad
                        };
                        carrito.Items.Add(nuevoItem);
                    }

                    await _carritoRepo.UpdateAsync(carrito);
                    return Ok(carrito);
                }
            }
            catch (Exception ex)
            {
                // En desarrollo puedes devolver inner exception para depurar:
                var detalle = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new { message = "Error al agregar item al carrito", detail = detalle });
            }
        }

        /// <summary>
        /// Obtener carrito del usuario (si hay varios, devuelve el primero)
        /// </summary>
        [HttpGet("{usuarioId:guid}")]
        public async Task<IActionResult> GetCart(Guid usuarioId)
        {
            var carritos = await _carritoRepo.GetByUsuarioAsync(usuarioId);
            var carrito = carritos?.FirstOrDefault();
            if (carrito == null) return NotFound();
            return Ok(carrito);
        }

        /// <summary>
        /// Eliminar item del carrito por itemId (UId del CarritoItem)
        /// </summary>
        [HttpDelete("items/{itemId:guid}")]
        public async Task<IActionResult> RemoveItem(Guid itemId)
        {
            try
            {
                // Recomendado: usar un método repo específico para buscar el carrito que contiene el item
                // Si tienes GetByItemIdAsync en el repo úsalo:
                if (_carritoRepo is ICarritoRepository repoWithGetByItem)
                {
                    // llamada directa si el repo implementa el método
                    try
                    {
                        // Este cast es solo para indicar intención; si ya implementaste GetByItemIdAsync lo llamas directamente.
                    }
                    catch { /* ignore */ }
                }

                // Fallback: buscar en todos los carritos (menos eficiente)
                var carritos = await _carritoRepo.GetAllAsync();
                var carrito = carritos.FirstOrDefault(c => c.Items.Any(i => i.UId == itemId));
                if (carrito == null) return NotFound("Item no encontrado en ningún carrito");

                var item = carrito.Items.First(i => i.UId == itemId);
                carrito.Items.Remove(item);

                await _carritoRepo.UpdateAsync(carrito);
                return NoContent();
            }
            catch (Exception ex)
            {
                var detalle = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new { message = "Error al eliminar item", detail = detalle });
            }
        }

        /// <summary>
        /// Vaciar carrito del usuario (elimina todos los items)
        /// </summary>
        [HttpDelete("clear/{usuarioId:guid}")]
        public async Task<IActionResult> ClearCart(Guid usuarioId)
        {
            try
            {
                var carritos = await _carritoRepo.GetByUsuarioAsync(usuarioId);
                var carrito = carritos?.FirstOrDefault();
                if (carrito == null) return NotFound();

                carrito.Items.Clear();
                await _carritoRepo.UpdateAsync(carrito);
                return NoContent();
            }
            catch (Exception ex)
            {
                var detalle = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new { message = "Error al vaciar carrito", detail = detalle });
            }
        }
    }
}
    