using MvcNetCoreLinqToSql.Models;
using System.Data;
using System.Data.SqlClient;

namespace MvcNetCoreLinqToSql.Repositories
{
    public class RepositoryEmpleados
    {
        private DataTable tablaEmpleados;

        public RepositoryEmpleados()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;User ID=sa;Password=MSCD2023;Encrypt=False";
            string sql = "SELECT * FROM EMP";
            SqlDataAdapter adapter = new SqlDataAdapter(connectionString, sql);
            this.tablaEmpleados = new DataTable();
            adapter.Fill(tablaEmpleados);
        }

        public List<Empleado> GetEmpleados()
        {
            // La consulta LINQ se almacena en variables de tipo var
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           select datos;
            // Lo almacenado en consulta es un conjutno de objetos DataRow, los
            // objetos que contiene la clase DataTable
            // Debemos convertir dichos DataRow en empleados
            List<Empleado> empleados = new List<Empleado>();
            // Recorremos cada fila de la consulta
            foreach (var row in consulta)
            {
                // Para extraer los datos de una fila DataRow
                // fila.Field<Tipo>("COLUMNA")
                Empleado empleado = new Empleado();
                empleado.IdEmpleado = row.Field<int>("EMP_NO");
                empleado.Apellido = row.Field<string>("APELLIDO");
                empleado.IdDepartamento = row.Field<int>("DEPT_NO");
                empleado.Oficio = row.Field<string>("OFICIO");
                empleado.Salario = row.Field<int>("SALARIO");
                empleados.Add(empleado);
            }
            return empleados;
        }

        public Empleado FindEmpleado(int idempleado)
        {
            // El alias datos representa cada objeto dentro del conjunto
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<int>("EMP_NO") == idempleado
                           select datos;
            // consulta siempre será una colección, aunque solo haya un row
            // consulta contiene una serie de métodos LAMBDA Para indiciar
            // ciertas filas o acciones necesarias
            // Tenemos un método llamado First() que nos devuelve la primera fila
            var row = consulta.First();
            Empleado empleado = new Empleado();
            empleado.IdEmpleado = row.Field<int>("EMP_NO");
            empleado.Apellido = row.Field<string>("APELLIDO");
            empleado.IdDepartamento = row.Field<int>("DEPT_NO");
            empleado.Oficio = row.Field<string>("OFICIO");
            empleado.Salario = row.Field<int>("SALARIO");
            return empleado;
        }
    }
}
