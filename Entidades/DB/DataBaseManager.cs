using System.Data.SqlClient;
using System.Linq.Expressions;
using Entidades.Excepciones;
using Entidades.Exceptions;
using Entidades.Files;
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


        /// <summary>
        /// Obtiene la imagen de la comida segun el string recibido
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        /// <exception cref="DataBaseManagerException"></exception>
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
                    else
                    {
                       throw new ComidaInvalidaExeption("No se pudo Obtener la imagen de la base de datos");  
                    }  
                }
                
            }
            catch (DataBaseManagerException ex)
            {
                FileManager.Guardar(ex.Message + ex.StackTrace+ex.GetType(), "logs.txt", true);
                return img;
            
            }
            catch(ComidaInvalidaExeption ex) 
            {
                FileManager.Guardar(ex.Message + ex.StackTrace + ex.GetType(), "logs.txt", true);
                return img;
            }
            catch (Exception ex)
            {
                throw new DataBaseManagerException("No se pudo leer en la base de datos",ex);
            }

        }
        /// <summary>
        /// Guarda el tickket en la base de datos
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nombreEmpleado"></param>
        /// <param name="comida"></param>
        /// <returns></returns>
        /// <exception cref="DataBaseManagerException"></exception>
        public static bool GuardarTicket<T>(string nombreEmpleado,T comida) where T : IComestible,new()
        {
            try
            {
                using (DataBaseManager.connection = new SqlConnection(DataBaseManager.stringConnection))
                {   
                    
                    string consulta = "INSERT INTO tickets (empleado,ticket) VALUES(@0,@1)";

                    SqlCommand command = new SqlCommand(consulta, DataBaseManager.connection);
                    DataBaseManager.connection.Open();
                    command.Parameters.AddWithValue("@0",nombreEmpleado);
                    command.Parameters.AddWithValue("@1",comida.ToString());
                    if (command.ExecuteNonQuery()>0)
                    {
                       return true;
                    }
                    else
                    {
                       throw new DataBaseManagerException("No se pudo escribir en la base de datos");
                    }
                }
            }
            catch (DataBaseManagerException ex) 
            {
                FileManager.Guardar(ex.Message + ex.StackTrace + ex.GetType(), "logs.txt", true);
                return false;
            }
            catch(Exception ex)
            {
                throw new DataBaseManagerException("No se pudo leer en la base de datos", ex);
            }

            
            
        }

    }
}
