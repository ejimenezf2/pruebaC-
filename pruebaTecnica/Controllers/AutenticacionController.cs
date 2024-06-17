using Microsoft.AspNetCore.Mvc;
using pruebaTecnica.Modelos;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using pruebaTecnica.Data;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace pruebaTecnica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string secretkey;

        public AutenticacionController(IConfiguration configuration)
        {
            _configuration = configuration;
            secretkey = _configuration["settings:secretkey"] ?? throw new ArgumentNullException("secretkey", "Secret key cannot be null.");
            if (secretkey.Length < 32)
            {
                throw new ArgumentException("The secret key must be at least 32 characters long.");
            }
        }

        [HttpPost]
        [Route("Validar")]
        public async Task<IActionResult> Validar([FromBody] Usuario request)
        {
            if (string.IsNullOrEmpty(request.Correo))
            {
                return BadRequest(new { message = "El correo es requerido." });
            }

            UsuarioData usuarioData = new UsuarioData(_configuration);
            Usuario usuario = await usuarioData.UsuarioCorreo(request.Correo);

            if (usuario == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "Usuario no encontrado." });
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, usuario.Password))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "Contraseña incorrecta." });
            }

            var keyBytes = Encoding.ASCII.GetBytes(secretkey);
            var claims = new ClaimsIdentity();

            if (!string.IsNullOrEmpty(usuario.IdUsuario.ToString()) && !string.IsNullOrEmpty(usuario.Correo))
            {
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()));
                claims.AddClaim(new Claim(ClaimTypes.Email, usuario.Correo));
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al procesar las reclamaciones del usuario." });
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
            string tokenCreado = tokenHandler.WriteToken(tokenConfig);

            return StatusCode(StatusCodes.Status200OK, new { tokenCreado });
        }
    }
}
