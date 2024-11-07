using CitasMedicas.RecepcionistaApi.Data;
using CitasMedicas.RecepcionistaApi.DTO;
using CitasMedicas.RecepcionistaApi.Model;
using CitasMedicas.RecepcionistaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitasMedicas.RecepcionistaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecepcionistaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly CorrelativoService _correlativoService;

        public RecepcionistaController(ApplicationDbContext context, CorrelativoService correlativoService)
        {
            _context = context;
            _correlativoService = correlativoService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RegistrarRecepcionista([FromBody] RecepcionistaRequestDto recepcionistaRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            // Iniciar una transacción
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                string codigoRecepcionista = await _correlativoService.ObtenerNuevoCorrelativoAsync("CR");

                Recepcionista recepcionista = new Recepcionista
                {
                    IdUsuario = recepcionistaRequestDto.IdUsuario,
                    CodigoRecepcionista = codigoRecepcionista,
                    FechaContratacion = recepcionistaRequestDto.FechaContratacion,
                    Turno = recepcionistaRequestDto.Turno,
                    Departamento = recepcionistaRequestDto.Departamento,                    
                    EsActivo = recepcionistaRequestDto.EsActivo,
                    UsuarioCreacion = recepcionistaRequestDto.UsuarioCreacion,
                    FechaCreacion = DateTime.Now
                };

                // Guardar el médico en la base de datos
                _context.Recepcionistas.Add(recepcionista);
                await _context.SaveChangesAsync();

                RecepcionistaResponseDto recepcionistaResponseDto = new RecepcionistaResponseDto
                {
                    IdRecepcionista = recepcionista.IdRecepcionista,
                    IdUsuario = recepcionista.IdUsuario,
                    CodigoRecepcionista = recepcionista.CodigoRecepcionista,
                    FechaContratacion = recepcionista.FechaContratacion,
                    Turno = recepcionista.Turno,
                    Departamento = recepcionista.Departamento,                    
                    EsActivo = recepcionista.EsActivo,
                };

                // Confirmar la transacción
                await transaction.CommitAsync();

                return Ok(recepcionistaResponseDto);
            }
            catch (Exception ex)
            {
                // Deshacer la transacción en caso de error
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error en el servidor: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> EditarRecepcionista(int id, [FromBody] RecepcionistaRequestDto recepcionistaRequestDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                Recepcionista? recepcionista = await _context.Recepcionistas.FirstOrDefaultAsync(m => m.IdRecepcionista == id);

                if (recepcionista == null)
                {
                    return NotFound(); // Retorna 404 si el médico no existe
                }

                recepcionista.FechaContratacion = recepcionistaRequestDto.FechaContratacion;

                if (!string.IsNullOrEmpty(recepcionistaRequestDto.Turno))
                {
                    recepcionista.Turno = recepcionistaRequestDto.Turno;
                }

                if (!string.IsNullOrEmpty(recepcionistaRequestDto.Departamento))
                {
                    recepcionista.Departamento = recepcionistaRequestDto.Departamento;
                }

                recepcionista.UsuarioModificacion = recepcionistaRequestDto.UsuarioModificacion;
                recepcionista.FechaModificacion = DateTime.Now;

                _context.Recepcionistas.Update(recepcionista);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Revertir la transacción en caso de error
                return StatusCode(500, "Error al actualizar el médico y sus horarios"); // Retorna un código 500 en caso de error
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<RecepcionistaResponseDto>> ObtenerRecepcionista(int id)
        {
            var recepcionista = await _context.Recepcionistas
                .Where(r => r.IdRecepcionista == id)
                .Select(m => new RecepcionistaResponseDto
                {
                    IdRecepcionista = m.IdRecepcionista,
                    IdUsuario = m.IdUsuario,
                    CodigoRecepcionista = m.CodigoRecepcionista,
                    FechaContratacion = m.FechaContratacion,
                    Turno = m.Turno,
                    Departamento = m.Departamento,                    
                    EsActivo = m.EsActivo,
                })
                .FirstOrDefaultAsync();

            if (recepcionista == null)
            {
                return NotFound();
            }

            return Ok(recepcionista);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecepcionistaResponseDto>>> ObtenerCitasMedicas()
        {
            var recepcionistas = await _context.Recepcionistas
                .Select(m => new RecepcionistaResponseDto
                {
                    IdRecepcionista = m.IdRecepcionista,
                    IdUsuario = m.IdUsuario,
                    CodigoRecepcionista = m.CodigoRecepcionista,
                    FechaContratacion = m.FechaContratacion,
                    Turno = m.Turno,
                    Departamento = m.Departamento,                    
                    EsActivo = m.EsActivo,
                })
                .ToListAsync();

            return Ok(recepcionistas);
        }
    }
}