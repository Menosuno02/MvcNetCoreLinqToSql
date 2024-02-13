using MvcNetCoreLinqToSql.Models;
using System.Data;
using System.Data.SqlClient;

namespace MvcNetCoreLinqToSql.Repositories
{
    public class RepositoryEnfermos
    {
        private DataTable tablaEnfermos;
        SqlConnection cn;
        SqlCommand com;

        public RepositoryEnfermos()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Password=MCSD2023;Encrypt=False";
            string sql = "SELECT * FROM ENFERMO";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connectionString);
            this.tablaEnfermos = new DataTable();
            adapter.Fill(this.tablaEnfermos);
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<Enfermo> GetEnfermos()
        {
            var consulta = from datos in tablaEnfermos.AsEnumerable()
                           select datos;
            List<Enfermo> enfermos = new List<Enfermo>();
            foreach (var row in consulta)
            {
                Enfermo enfermo = new Enfermo
                {
                    Apellido = row.Field<string>("APELLIDO"),
                    Inscripcion = row.Field<int>("INSCRIPCION"),
                    Direccion = row.Field<string>("DIRECCION"),
                    FechaNacimiento = row.Field<DateTime>("FECHA_NAC"),
                    Sexo = row.Field<string>("S"),
                    NSS = row.Field<int>("NSS")
                };
                enfermos.Add(enfermo);
            }
            return enfermos;
        }

        public Enfermo FindEnfermo(int id)
        {
            var consulta = from datos in tablaEnfermos.AsEnumerable()
                           where datos.Field<int>("INSCRIPCION") == id
                           select datos;
            var row = consulta.First();
            Enfermo enfermo = new Enfermo
            {
                Apellido = row.Field<string>("APELLIDO"),
                Inscripcion = row.Field<int>("INSCRIPCION"),
                Direccion = row.Field<string>("DIRECCION"),
                FechaNacimiento = row.Field<DateTime>("FECHA_NAC"),
                Sexo = row.Field<string>("S"),
                NSS = row.Field<int>("NSS")
            };
            return enfermo;
        }

        public async Task DeleteEnfermo(int id)
        {
            string sql = "DELETE FROM ENFERMO WHERE INSCRIPCION = @INSCRIPCION";
            this.com.Parameters.AddWithValue("@INSCRIPCION", id);
            this.com.CommandText = sql;
            this.com.CommandType = CommandType.Text;
            await this.cn.OpenAsync();
            int result = await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }
    }
}
