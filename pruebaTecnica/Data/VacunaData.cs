using System.Data;
using System.Data.SqlClient;
using pruebaTecnica.Modelos;
namespace pruebaTecnica.Data
{
    public class VacunaData
    {
        private readonly string conexion;

        public VacunaData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL") ?? throw new ArgumentNullException("Connection string 'CadenaSQL' not found.");
        }
        public async Task<List<Vacuna>> Lista()
        {
            List<Vacuna> lista = new List<Vacuna>();
            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listavacuna", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Vacuna
                        {
                            Id_Vacuna= Convert.ToInt32(reader["id_vacuna"]),
                            Nombre = reader["nombre"].ToString(),
                        });
                    }
                }
            }
            return lista;
        }
        public async Task<Vacuna> Obtener(int Id)
        {
            Vacuna objeto = new Vacuna();
            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_obtenerVacuna", con);
                cmd.Parameters.AddWithValue("@id_vacuna", Id);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        objeto = new Vacuna
                        {
                            Id_Vacuna = Convert.ToInt32(reader["id_vacuna"]),
                            Nombre = reader["nombre"].ToString(),
                        };
                    }
                }
            }
            return objeto;
        }
        public async Task<bool> Crear(Vacuna objeto)
        {
            bool respuesta = true;
            using (var con = new SqlConnection(conexion))
            {

                SqlCommand cmd = new SqlCommand("sp_crearVacuna", con);
                cmd.Parameters.AddWithValue("@nombre", objeto.Nombre);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    await con.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
        public async Task<bool> Editar(Vacuna objeto)
        {
            bool respuesta = true;
            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_editarVacuna", con);
                cmd.Parameters.AddWithValue("@id_vacuna", objeto.Id_Vacuna);
                cmd.Parameters.AddWithValue("@nombre", objeto.Nombre);

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    await con.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
        public async Task<bool> Eliminar(int Id)
        {
            bool respuesta = true;
            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_eliminarVacuna", con);
                cmd.Parameters.AddWithValue("@id_vacuna", Id);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    await con.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
    }
}
