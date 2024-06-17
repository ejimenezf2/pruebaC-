using pruebaTecnica.Modelos;
using System.Data.SqlClient;
using System.Data;

namespace pruebaTecnica.Data
{
    public class EstadoVacunacionData
    {
        private readonly string conexion;

        public EstadoVacunacionData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL") ?? throw new ArgumentNullException("Connection string 'CadenaSQL' not found.");
        }
        public async Task<List<EstadoVacunacion>> Lista()
        {
            List<EstadoVacunacion> lista = new List<EstadoVacunacion>();
            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_MostrarEstadoVacunacion", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new EstadoVacunacion
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Nombre = reader["nombre"].ToString(),
                        });
                    }
                }
            }
            return lista;
        }
    }
}
