using pruebaTecnica.Modelos;
using System.Data.SqlClient;
using System.Data;

namespace pruebaTecnica.Data
{
    public class EmpleadoData
    {
        private readonly string conexion;
        public EmpleadoData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL") ?? throw new ArgumentNullException("Connection string 'CadenaSQL' not found.");
        }
        public async Task<List<Empleado>> Lista()
        {
            List<Empleado> lista = new List<Empleado>();
            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_mostrarEmpleado", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Empleado
                        {
                            Cod_Empleado = Convert.ToInt32(reader["cod_empleado"]),
                            Nombre = reader["nombre"].ToString(),
                            Apellido = reader["apellido"].ToString(),
                            Puesto_Laboral = reader["puesto_laboral"].ToString(),
                            Nombre_Vacuna = reader["nombre_vacuna"].ToString(),
                            Estado_Vacuna = reader["estado_vacuna"].ToString(),
                            Fecha_Primer_Dosis = reader["fecha_primer_dosis"] != DBNull.Value ? (DateTime?)reader["fecha_primer_dosis"] : null,
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<Empleado> Obtener(int Id)
        {
            Empleado objeto = new Empleado();
            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_obtenerEmpleado", con);
                cmd.Parameters.AddWithValue("@cod_empleado", Id);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        objeto = new Empleado
                        {

                            Cod_Empleado = reader["cod_empleado"] != DBNull.Value ? Convert.ToInt32(reader["cod_empleado"]) : 0,
                            Id_Vacuna = reader["id_vacuna"] != DBNull.Value ? (int?)Convert.ToInt32(reader["id_vacuna"]) : null,
                            Estado_Vacunacion = reader["estado_vacunacion"] != DBNull.Value ? (int?)Convert.ToInt32(reader["estado_vacunacion"]) : null,
                            Nombre = reader["nombre"] != DBNull.Value ? reader["nombre"].ToString() : null,
                            Apellido = reader["apellido"] != DBNull.Value ? reader["apellido"].ToString() : null,
                            Puesto_Laboral = reader["puesto_laboral"] != DBNull.Value ? reader["puesto_laboral"].ToString() : null,
                            Nombre_Vacuna = reader["nombre_vacuna"] != DBNull.Value ? reader["nombre_vacuna"].ToString() : null,
                            Estado_Vacuna = reader["estado_vacuna"] != DBNull.Value ? reader["estado_vacuna"].ToString() : null,
                            Fecha_Primer_Dosis = reader["fecha_primer_dosis"] != DBNull.Value ? (DateTime?)reader["fecha_primer_dosis"] : null

                        };
                    }
                }
            }
            return objeto;
        }
        public async Task<bool> Crear(Empleado objeto)
        {
            bool respuesta = true;
            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_crearEmpleado", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cod_empleado", objeto.Cod_Empleado);
                cmd.Parameters.AddWithValue("@nombre", objeto.Nombre ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@apellido", objeto.Apellido ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@puesto_laboral", objeto.Puesto_Laboral ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@id_vacuna", objeto.Id_Vacuna ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@fecha_primer_dosis", objeto.Fecha_Primer_Dosis ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@estado_vacunacion", objeto.Estado_Vacunacion ?? (object)DBNull.Value);

                try
                {
                    await con.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0;
                }
                catch (Exception ex)
                {
                    // Puedes manejar el error de alguna manera o registrar el error
                    Console.WriteLine(ex.Message);
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public async Task<bool> Editar(Empleado objeto)
        {
            bool respuesta = true;
            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_editarEmpleado", con);

                cmd.Parameters.AddWithValue("@cod_empleado", objeto.Cod_Empleado);
                cmd.Parameters.AddWithValue("@nombre", objeto.Nombre);
                cmd.Parameters.AddWithValue("@apellido", objeto.Apellido);
                cmd.Parameters.AddWithValue("@puesto_laboral", objeto.Puesto_Laboral);
                cmd.Parameters.AddWithValue("@id_vacuna", objeto.Id_Vacuna ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@fecha_primer_dosis", objeto.Fecha_Primer_Dosis ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@estado_vacunacion", objeto.Estado_Vacunacion ?? (object)DBNull.Value);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    await con.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0;
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
                SqlCommand cmd = new SqlCommand("sp_cambiarEstadoEmpleado", con);
                cmd.Parameters.AddWithValue("@cod_empleado", Id);
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
