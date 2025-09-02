using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using System.Linq;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdenController : ControllerBase
    {
        private readonly IOrdenRepository _ordenRepo;
        private readonly ICarritoRepository _carritoRepo;

        public OrdenController(IOrdenRepository ordenRepo, ICarritoRepository carritoRepo)
        {
            _ordenRepo = ordenRepo;
            _carritoRepo = carritoRepo;
        }

        /// <summary>
        /// Crear orden (simple): recibe la orden en el body y la persiste.
        /// Nota: para checkout completo (pago + stock) crea un UseCase/Service transaccional.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Orden orden)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                // Se asume que orden.UId fue generado por la entidad (Guid.NewGuid()).
                await _ordenRepo.AddAsync(orden);

                return CreatedAtAction(nameof(GetById), new { id = orden.UId }, orden);
            }
            catch (Exception ex)
            {
                // Agrega ILogger si quieres loggear el error
                return StatusCode(500, new { message = "Error al crear la orden", detail = ex.Message });
            }
        }

        /// <summary>
        /// Obtener orden por id (guid)
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var o = await _ordenRepo.GetByIdAsync(id);
            if (o == null) return NotFound();
            return Ok(o);
        }

        /// <summary>
        /// Listar órdenes por cliente (usuario)
        /// </summary>
        [HttpGet("cliente/{clienteId:guid}")]
        public async Task<IActionResult> GetByCliente(Guid clienteId)
        {
            var list = await _ordenRepo.GetByUsuarioAsync(clienteId);
            return Ok(list);
        }

        /*
         * EJEMPLO (opcional) — Crear orden desde el carrito (checkout simplificado)
         * 
         * Si quieres manejar el flujo completo de checkout, no lo pongas en el controller:
         * crea un OrderService/UseCase que haga: validar stock, crear la orden, reservar/reducir stock,
         * procesar el pago (IPaymentGateway), guardar PaymentRecord, confirmar o revertir.
         * 
         * El fragmento siguiente es sólo ilustrativo de cómo podrías construir la orden desde el carrito.
         * Descomenta/adapta si implementas el servicio/transacciones.
        */
        /*
        [HttpPost("checkout/{usuarioId:guid}")]
        public async Task<IActionResult> Checkout(Guid usuarioId, [FromBody] CheckoutDto checkout)
        {
            // Ejemplo simple: obtener carrito del usuario, sumar totales, crear orden y limpiar carrito.
            var carritos = await _carritoRepo.GetByUsuarioAsync(usuarioId);
            var carrito = carritos?.FirstOrDefault();
            if (carrito == null || !carrito.Items.Any())
                return BadRequest("Carrito vacío");

            // Aquí deberías: validar stock por cada item, reservar stock, procesar pago (gateway), etc.
            // Montar la orden:
            var orden = new Orden
            {
                UsuarioId = usuarioId,
                FechaCreacion = DateTime.UtcNow,
                Estado = "Pendiente",
                Total = carrito.Items.Sum(i => /* obtener precio desde producto o snapshot 0m)
            };

          // Agregar la lógica de persistencia + pago + ajuste de inventario en un UseCase transaccional.
              await _ordenRepo.AddAsync(orden);

              // Finalmente limpiar o actualizar el carrito:
              carrito.Items.Clear();
            await _carritoRepo.UpdateAsync(carrito);

            return CreatedAtAction(nameof(GetById), new { id = orden.UId
        }, orden);
         }
            */

    }
}
