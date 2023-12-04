using System.Data.SqlClient;
using System.Linq.Expressions;
using Entidades.Excepciones;
using Entidades.Exceptions;
using Entidades.Interfaces;

namespace Entidades.DataBase
{
    static class DataBaseManager 
    {
        static SqlConnection connection;
        static string stringConnection;

        static DataBaseManager()
        {
            stringConnection = "Server=.;Database=20230622SP;Trusted_Connection=True";
            connection = new SqlConnection(stringConnection);
        }

        public static string GetImagenComida(string tipo)
        {
            string img="";

            try
            {
                using(DataBaseManager.connection = new SqlConnection(DataBaseManager.stringConnection))
                {
                    string consulta = "SELECT imagen FROM Comidas WHERE tipo_comida = @0";
                    SqlCommand command= new SqlCommand(consulta,DataBaseManager.connection);
                    DataBaseManager.connection.Open();
                    command.Parameters.AddWithValue("@0",tipo);
                    SqlDataReader sqlDataReader = command.ExecuteReader();

                    if(sqlDataReader.Read())
                    {
                        img=sqlDataReader.GetString(0);
                        return img;
                    }

                }
                return img;
            }
            catch (Exception ex)
            {
                throw new DataBaseManagerException("Error de lectura de la base de datos",ex);
            }
   
        }

        public static bool GuardarTicket<T>(string nombreEmpleado,T comida) where T : IComestible,new()
        {
            try
            {
                using (DataBaseManager.connection = new SqlConnection(DataBaseManager.stringConnection))
                {   
                    
                    string consulta = "INSERT INTO tickets (empleado,ticket) VALUES(@0,@1)";

                    SqlCommand command = new SqlCommand(consulta, DataBaseManager.connection);
                    command.Parameters.AddWithValue("@0",nombreEmpleado);
                    command.Parameters.AddWithValue("@1",comida.ToString());
                    DataBaseManager.connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) 
            {
                throw new DataBaseManagerException("No se pudo escribir en la base de datos", ex);
            }

            
            
        }

    }
}
