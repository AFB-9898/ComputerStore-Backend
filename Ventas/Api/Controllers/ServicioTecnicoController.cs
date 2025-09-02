using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicioTecnicoController : ControllerBase
    {
        // Repositorios inyectados
        private readonly IServicioTecnicoRepository _repo;
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly ITecnicoRepository _tecnicoRepo;

        // Constructor inyección de dependencias
        public ServicioTecnicoController(IServicioTecnicoRepository repo, IUsuarioRepository usuarioRepo, ITecnicoRepository tecnicoRepo)
        {
            _repo = repo;
            _usuarioRepo = usuarioRepo;
            _tecnicoRepo = tecnicoRepo;
        }

        // GET: listar todos los servicios
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repo.GetAllAsync());

        // GET por Id
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var servicio = await _repo.GetByIdAsync(id);
            if (servicio == null) return NotFound(); // Validación existencia
            return Ok(servicio);
        }

        // POST: crear servicio técnico
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServicioTecnico entity)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // Validación DTO

            // Validar existencia de usuario y técnico
            var usuario = await _usuarioRepo.GetByIdAsync(entity.UsuarioId);
            if (usuario == null) return BadRequest($"No existe usuario con Id {entity.UsuarioId}");

            var tecnico = await _tecnicoRepo.GetByIdAsync(entity.TecnicoId);
            if (tecnico == null) return BadRequest($"No existe técnico con Id {entity.TecnicoId}");

            // Generar UId y asignar referencias
            entity.UId = Guid.NewGuid();
            entity.Usuario = usuario;
            entity.Tecnico = tecnico;

            try
            {
                await _repo.AddAsync(entity); // Guardar en DB
                return CreatedAtAction(nameof(GetById), new { id = entity.UId }, entity); // Retornar 201
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear servicio técnico", detail = ex.Message }); // Error genérico
            }
        }

        // PUT: actualizar servicio técnico
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ServicioTecnico entity)
        {
            if (id != entity.UId) return BadRequest("Id no coincide"); // Validar Id

            // Validar referencias actualizadas
            var usuario = await _usuarioRepo.GetByIdAsync(entity.UsuarioId);
            if (usuario == null) return BadRequest($"No existe usuario con Id {entity.UsuarioId}");

            var tecnico = await _tecnicoRepo.GetByIdAsync(entity.TecnicoId);
            if (tecnico == null) return BadRequest($"No existe técnico con Id {entity.TecnicoId}");

            entity.Usuario = usuario;
            entity.Tecnico = tecnico;

            try
            {
                await _repo.UpdateAsync(entity); // Guardar cambios
                return NoContent(); // 204: actualizado
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar servicio técnico", detail = ex.Message });
            }
        }

        // DELETE: eliminar servicio técnico
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _repo.DeleteAsync(id); // Eliminar de DB
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar servicio técnico", detail = ex.Message });
            }
        }

        // GET servicios por usuario
        [HttpGet("usuario/{usuarioId:guid}")]
        public async Task<IActionResult> GetByUsuario(Guid usuarioId)
        {
            var all = await _repo.GetAllAsync();
            var filtered = all.Where(s => s.UsuarioId == usuarioId); // Filtrado
            return Ok(filtered);
        }

        // GET servicios por técnico
        [HttpGet("tecnico/{tecnicoId:guid}")]
        public async Task<IActionResult> GetByTecnico(Guid tecnicoId)
        {
            var all = await _repo.GetAllAsync();
            var filtered = all.Where(s => s.TecnicoId == tecnicoId); // Filtrado
            return Ok(filtered);
        }
    }
}
