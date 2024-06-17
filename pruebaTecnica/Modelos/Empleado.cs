namespace pruebaTecnica.Modelos
{
    public class Empleado
    {
        public int Cod_Empleado { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public DateTime Fecha_Creacion { get; set; }
        public string? Puesto_Laboral { get; set; }
        public int? Id_Vacuna { get; set; } 
        public DateTime? Fecha_Primer_Dosis { get; set; } 
        public int? Estado_Vacunacion { get; set; }
        public string? Nombre_Vacuna { get; set; } 
        public string? Estado_Vacuna { get; set; }
    }
}
