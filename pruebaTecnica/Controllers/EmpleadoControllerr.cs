using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pruebaTecnica.Data;
using pruebaTecnica.Modelos;

namespace pruebaTecnica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly EmpleadoData _empleadoData;
        public EmpleadoController(EmpleadoData empleadoData)
        {
            _empleadoData = empleadoData;
        }

        [HttpGet("Show")]
        public async Task<IActionResult> Lista()
        {
            List<Empleado> lista = await _empleadoData.Lista();
            return StatusCode(StatusCodes.Status200OK, lista);
        }

        [HttpGet("Obtener/{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            Empleado objeto = await _empleadoData.Obtener(id);
            return StatusCode(StatusCodes.Status200OK, objeto);
        }
        [HttpGet("Reporte/{id}")]
        public async Task<IActionResult> Reporte(int id)
        {
            Empleado objeto = await _empleadoData.Reporte(id);
            return StatusCode(StatusCodes.Status200OK, objeto);
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] Empleado objeto)
        {
            bool respuesta = await _empleadoData.Crear(objeto);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }

        [HttpPut("Editar")]
        public async Task<IActionResult> Editar([FromBody] Empleado objeto)
        {
            bool respuesta = await _empleadoData.Editar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }

        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool respuesta = await _empleadoData.Eliminar(id);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }
    }
}
