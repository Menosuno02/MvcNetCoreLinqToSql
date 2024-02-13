namespace MvcNetCoreLinqToSql.Models
{
    public class Enfermo
    {
        public int Inscripcion { get; set; }
        public string Apellido { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public int NSS { get; set; }
    }
}
