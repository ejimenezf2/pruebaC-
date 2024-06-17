using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pruebaTecnica.Data;
using pruebaTecnica.Modelos;

namespace pruebaTecnica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoVacunacionController : ControllerBase
    {
        private readonly EstadoVacunacionData _EstadoVacunacionData;

        public EstadoVacunacionController(EstadoVacunacionData estadoVacunacionData)
        {
            _EstadoVacunacionData = estadoVacunacionData;
        }

        [HttpGet("Show")]
        public async Task<IActionResult> Lista()
        {
            List<EstadoVacunacion> lista = await _EstadoVacunacionData.Lista();
            return StatusCode(StatusCodes.Status200OK, lista);
        }
    }
}
