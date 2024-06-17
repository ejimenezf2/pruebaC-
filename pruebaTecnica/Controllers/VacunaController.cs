using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pruebaTecnica.Modelos;
using pruebaTecnica.Data;

namespace pruebaTecnica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacunaController : ControllerBase
    {
        private readonly VacunaData _vacunaData;

        public VacunaController(VacunaData vacunaData)
        {
            _vacunaData = vacunaData;
        }

        [HttpGet("Show")]
        public async Task<IActionResult> Lista()
        {
            List<Vacuna> lista = await _vacunaData.Lista();
            return StatusCode(StatusCodes.Status200OK, lista);
        }
        [HttpGet("obtener/{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            Vacuna objeto = await _vacunaData.Obtener(id);
            return StatusCode(StatusCodes.Status200OK, objeto);
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] Vacuna objeto)
        {
            bool respuesta = await _vacunaData.Crear(objeto);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }

        [HttpPut("Editar")]
        public async Task<IActionResult> Editar([FromBody] Vacuna objeto)
        {
            bool respuesta = await _vacunaData.Editar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }

        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool respuesta = await _vacunaData.Eliminar(id);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }
    }
}
