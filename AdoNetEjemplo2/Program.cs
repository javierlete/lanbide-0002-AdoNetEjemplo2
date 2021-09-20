using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetEjemplo2
{
    class Program
    {
        static void Main()
        {
            UsuariosDataSet ds = new UsuariosDataSet();

            UsuariosDataSetTableAdapters.UsuariosTableAdapter ta = new UsuariosDataSetTableAdapters.UsuariosTableAdapter();

            ta.Fill(ds.Usuarios);

            foreach(UsuariosDataSet.UsuariosRow dr in ds.Usuarios.Rows)
            {
                Console.WriteLine($"{dr.Id}, {dr.Email}, {dr.Password}");
            }

            UsuariosDataSet.UsuariosRow nueva = ds.Usuarios.NewUsuariosRow();

            nueva.Id = 0;
            nueva.Email = "nuevo2@email.net";
            nueva.Password = "nuevo2";

            ds.Usuarios.Rows.Add(nueva);

            UsuariosDataSet.UsuariosRow modificada = ds.Usuarios.FindById(2L);

            modificada.Password = "Modificada2";

            ds.Usuarios.FindById(4L).Delete();

            ta.Update(ds.Usuarios);
        }

        static void MainDesconectadoSinTipos(string[] args)
        {
            DataSet ds = new DataSet();

            IDbConnection con = new SqlConnection(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=mf0966;Integrated Security=True");

            IDbDataAdapter da = new SqlDataAdapter("SELECT * FROM Usuarios", (SqlConnection)con);

            da.DeleteCommand = con.CreateCommand();
            da.DeleteCommand.CommandText = "DELETE FROM Usuarios WHERE Id = @Id";
            
            IDbDataParameter parIdDelete = da.DeleteCommand.CreateParameter();
            parIdDelete.ParameterName = "@Id";
            parIdDelete.SourceColumn = "Id";
            parIdDelete.DbType = DbType.Int64;

            da.DeleteCommand.Parameters.Add(parIdDelete);

            da.UpdateCommand = con.CreateCommand();
            da.UpdateCommand.CommandText = "UPDATE Usuarios SET Email = @Email, Password = @Password WHERE Id = @Id";

            IDbDataParameter parEmailUpdate = da.UpdateCommand.CreateParameter();
            parEmailUpdate.ParameterName = "@Email";
            parEmailUpdate.SourceColumn = "Email";
            parEmailUpdate.DbType = DbType.String;

            da.UpdateCommand.Parameters.Add(parEmailUpdate);

            IDbDataParameter parPasswordUpdate = da.UpdateCommand.CreateParameter();
            parPasswordUpdate.ParameterName = "@Password";
            parPasswordUpdate.SourceColumn = "Password";
            parPasswordUpdate.DbType = DbType.String;

            da.UpdateCommand.Parameters.Add(parPasswordUpdate);

            IDbDataParameter parIdUpdate = da.DeleteCommand.CreateParameter();
            parIdUpdate.ParameterName = "@Id";
            parIdUpdate.SourceColumn = "Id";
            parIdUpdate.DbType = DbType.Int64;

            da.UpdateCommand.Parameters.Add(parIdUpdate);

            da.InsertCommand = con.CreateCommand();
            da.InsertCommand.CommandText = "INSERT INTO Usuarios (Email, Password) VALUES (@Email, @Password)";

            IDbDataParameter parEmailInsert = da.UpdateCommand.CreateParameter();
            parEmailInsert.ParameterName = "@Email";
            parEmailInsert.SourceColumn = "Email";
            parEmailInsert.DbType = DbType.String;

            da.InsertCommand.Parameters.Add(parEmailInsert);

            IDbDataParameter parPasswordInsert = da.UpdateCommand.CreateParameter();
            parPasswordInsert.ParameterName = "@Password";
            parPasswordInsert.SourceColumn = "Password";
            parPasswordInsert.DbType = DbType.String;

            da.InsertCommand.Parameters.Add(parPasswordInsert);

            da.Fill(ds); // Abre y cierra la conexión automáticamente

            DataTable usuarios = ds.Tables[0];

            usuarios.PrimaryKey = new DataColumn[] { usuarios.Columns["Id"] };

            foreach (DataRow dr in usuarios.Rows)
            {
                Console.WriteLine($"{dr["Id"]}, {dr["Email"]}, {dr["Password"]}");
            }

            DataRow nueva = usuarios.NewRow();

            nueva["Id"] = 0;
            nueva["Email"] = "nuevo@email.net";
            nueva["Password"] = "nuevo";

            usuarios.Rows.Add(nueva);

            DataRow modificada = usuarios.Rows.Find(2L);

            modificada["Password"] = "Modificada";

            usuarios.Rows.Find(3L).Delete();

            da.Update(ds);
        }
    }
}
