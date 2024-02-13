using MvcNetCoreLinqToSql.Models;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

namespace MvcNetCoreLinqToSql.Repositories
{
    public class RepositoryEmpleados
    {
        private DataTable tablaEmpleados;
        private DataTable tablaDepartamentos;

        public RepositoryEmpleados()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Password=MCSD2023;Encrypt=False";
            string sql = "SELECT * FROM EMP";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connectionString);
            this.tablaEmpleados = new DataTable();
            adapter.Fill(tablaEmpleados);
            sql = "SELECT * FROM DEPT";
            adapter = new SqlDataAdapter(sql, connectionString);
            this.tablaDepartamentos = new DataTable();
            adapter.Fill(tablaDepartamentos);
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

        public List<Empleado> GetEmpleadosOficioSalario(string oficio, int salario)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<string>("OFICIO") == oficio
                           && datos.Field<int>("SALARIO") >= salario
                           select datos;
            List<Empleado> empleados = new List<Empleado>(); ;
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                foreach (var row in consulta)
                {
                    // Sintaxis para instanciar un objeto y
                    // rellenar sus propiedades a la vez
                    Empleado empleado = new Empleado
                    {
                        IdEmpleado = row.Field<int>("EMP_NO"),
                        Apellido = row.Field<string>("APELLIDO"),
                        Oficio = row.Field<string>("OFICIO"),
                        Salario = row.Field<int>("SALARIO"),
                        IdDepartamento = row.Field<int>("DEPT_NO")
                    };
                    empleados.Add(empleado);
                }
                return empleados;
            }
        }

        // Filtrar empleados por oficio
        public ResumenEmpleados GetEmpleadosOficio(string oficio)
        {
            var consulta = from datos in tablaEmpleados.AsEnumerable()
                           where datos.Field<string>("OFICIO") == oficio
                           select datos;
            consulta = consulta.OrderBy(enfermo => enfermo.Field<int>("SALARIO"));
            int personas = consulta.Count();
            int maximo = consulta.Max(z => z.Field<int>("SALARIO"));
            double media = consulta.Average(x => x.Field<int>("SALARIO"));
            List<Empleado> empleados = new List<Empleado>();
            foreach (var row in consulta)
            {
                Empleado emple = new Empleado
                {
                    IdEmpleado = row.Field<int>("EMP_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Oficio = row.Field<string>("OFICIO"),
                    Salario = row.Field<int>("SALARIO"),
                    IdDepartamento = row.Field<int>("DEPT_NO"),

                };
                empleados.Add(emple);
            }
            ResumenEmpleados resumen = new ResumenEmpleados
            {
                MediaSalarial = media,
                MaximoSalario = maximo,
                Personas = personas,
                Empleados = empleados
            };
            return resumen;
        }

        public List<string> GetOficios()
        {
            var consulta = (from datos in tablaEmpleados.AsEnumerable()
                            select datos.Field<string>("OFICIO")).Distinct();
            List<string> oficios = new List<string>();
            foreach (var row in consulta)
            {
                oficios.Add(row);
            }
            return oficios;
        }

        /*
        public List<int> GetDepartamentos()
        {
            var consulta = (from datos in tablaEmpleados.AsEnumerable()
                            select datos.Field<int>("DEPT_NO")).Distinct();
            consulta = consulta.OrderBy(x => x);
            List<int> departamentos = new List<int>();
            foreach (var row in consulta)
            {
                departamentos.Add(row);
            }
            return departamentos;
        }
        */

        public List<Departamento> GetDepartamentos()
        {
            var consulta = from datos in tablaDepartamentos.AsEnumerable()
                           select datos;
            consulta = consulta.OrderBy(x => x.Field<int>("DEPT_NO"));
            List<Departamento> departamentos = new List<Departamento>();
            foreach (var row in consulta)
            {
                Departamento dept = new Departamento
                {
                    DeptNo = row.Field<int>("DEPT_NO"),
                    DeptNombre = row.Field<string>("DNOMBRE"),
                    Localidad = row.Field<string>("LOC"),
                };
                departamentos.Add(dept);
            }
            return departamentos;
        }

        public ResumenEmpleados GetEmpleadosDept(int dept_no)
        {
            var consulta = from datos in tablaEmpleados.AsEnumerable()
                           where datos.Field<int>("DEPT_NO") == dept_no
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            int personas = consulta.Count();
            int maximo = consulta.Max(x => x.Field<int>("SALARIO"));
            double media = consulta.Average(x => x.Field<int>("SALARIO"));
            List<Empleado> empleados = new List<Empleado>();
            foreach (var row in consulta)
            {
                Empleado emple = new Empleado
                {
                    Apellido = row.Field<string>("APELLIDO"),
                    IdEmpleado = row.Field<int>("EMP_NO"),
                    IdDepartamento = row.Field<int>("DEPT_NO"),
                    Oficio = row.Field<string>("OFICIO"),
                    Salario = row.Field<int>("SALARIO")
                };
                empleados.Add(emple);
            }
            ResumenEmpleados resumen = new ResumenEmpleados
            {
                Personas = personas,
                MaximoSalario = maximo,
                MediaSalarial = media,
                Empleados = empleados
            };
            return resumen;
        }
    }
}
