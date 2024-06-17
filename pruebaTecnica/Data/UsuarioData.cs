using pruebaTecnica.Modelos;
using System.Data;
using System.Data.SqlClient;
namespace pruebaTecnica.Data
{
    public class UsuarioData
    {
        private readonly string conexion;

        public UsuarioData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL") ?? throw new ArgumentNullException("Connection string 'CadenaSQL' no encontrada.");
        }
        public async Task<List<Usuario>> Lista()
        {
            List<Usuario> lista = new List<Usuario>();
            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listaUsuario", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Usuario
                        {
                            IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                            Nombre = reader["nombre"].ToString(),
                            Password = reader["password"].ToString(),
                            Correo = reader["correo"].ToString(),
                            FechaCreacion = reader["fecha_creacion"].ToString()
                        });
                    }
                }
            }
            return lista;
        }
        public async Task<Usuario> UsuarioCorreo(string correo)
        {
            if (string.IsNullOrEmpty(correo))
            {
                throw new ArgumentException("El correo no puede ser nulo o vacío.", nameof(correo));
            }

            Usuario usuario = null;
            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_obtenerUsuarioPorCorreo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@correo", correo);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        usuario = new Usuario
                        {
                            IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                            Nombre = reader["nombre"].ToString(),
                            Password = reader["password"].ToString(),
                            Correo = reader["correo"].ToString(),
                            FechaCreacion = reader["fecha_creacion"].ToString()
                        };
                    }
                }
            }
            return usuario;
        }

    }
}
