using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;
using System.Data;

namespace pryInicioSesionLogs
{
    internal class clsUsuario
    {
        OleDbConnection conexionBD;
        OleDbCommand comandoBD;
        OleDbDataReader lectorBD;

        OleDbDataAdapter adaptadorBD;
        DataSet objDS;

        string rutaArchivo;
        public string estadoConexion;
        private readonly string connectionString;

        public clsUsuario()
        {
            connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=../../Archivos/BDusuarios.accdb";
            try
            {
                rutaArchivo = @"../../Archivos/BDusuarios.accdb";

                conexionBD = new OleDbConnection();
                conexionBD.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + rutaArchivo;
                conexionBD.Open();

                objDS = new DataSet();

                estadoConexion = "Conectado";
            }
            catch (Exception error)
            {
                estadoConexion = error.Message;
            }
        }

        public void RegistroLogInicioSesion()
        {
            try
            {
                comandoBD = new OleDbCommand();

                comandoBD.Connection = conexionBD;
                comandoBD.CommandType = System.Data.CommandType.TableDirect;
                comandoBD.CommandText = "Logs";

                adaptadorBD = new OleDbDataAdapter(comandoBD);

                adaptadorBD.Fill(objDS, "Logs");

                DataTable objTabla = objDS.Tables["Logs"];
                DataRow nuevoRegistro = objTabla.NewRow();

                nuevoRegistro["Categoria"] = "Inicio Sesión";
                nuevoRegistro["FechaHora"] = DateTime.Now;
                nuevoRegistro["Descripcion"] = "Inicio exitoso";

                objTabla.Rows.Add(nuevoRegistro);

                OleDbCommandBuilder constructor = new OleDbCommandBuilder(adaptadorBD);
                adaptadorBD.Update(objDS, "Logs");

                estadoConexion = "Registro exitoso de log";
            }
            catch (Exception error)
            {

                estadoConexion = error.Message;
            }

        }

        public string ValidarUsuario(string nombreUser, string passUser)
        {
            try
            {
                comandoBD = new OleDbCommand();

                comandoBD.Connection = conexionBD;
                comandoBD.CommandType = System.Data.CommandType.Text;
                comandoBD.CommandText = "SELECT * FROM Usuario WHERE NombreUsuario = ? AND Password = ?";
                comandoBD.Parameters.AddWithValue("@NombreUsuario", nombreUser);
                comandoBD.Parameters.AddWithValue("@Password", passUser);

                lectorBD = comandoBD.ExecuteReader();

                if (lectorBD.HasRows)
                {
                    estadoConexion = "Usuario EXISTE";
                }
                else
                {
                    estadoConexion = "Usuario NO EXISTE";
                }
            }
            catch (Exception error)
            {
                estadoConexion = error.Message;
            }

            return estadoConexion;
        }

        public string InsertarUsuario(string nombreUsuario, string contraseña)
        {
            try
            {
                using (OleDbConnection conexionBD = new OleDbConnection(connectionString))
                {
                    conexionBD.Open();

                    using (OleDbCommand comandoBD = new OleDbCommand())
                    {
                        comandoBD.Connection = conexionBD;
                        comandoBD.CommandText = "INSERT INTO Usuario (NombreUsuario, Password) VALUES (@NombreUsuario, @Password)";
                        comandoBD.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                        comandoBD.Parameters.AddWithValue("@Password", contraseña);

                        comandoBD.ExecuteNonQuery();
                    }
                }

                estadoConexion = "Usuario insertado exitosamente";
            }
            catch (Exception error)
            {
                estadoConexion = error.Message;
            }

            return estadoConexion;
        }
    }
}