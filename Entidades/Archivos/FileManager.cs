using Entidades.Exceptions;
using Entidades.Interfaces;
using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entidades.Files
{

     public static class FileManager
     {
         static private string path;


        static FileManager()
        {
             path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+ "\\20230712_FranciscoHulej\\";

             ValidaExistenciaDeDirectorio();
        }

            /// <summary>
            /// Guarda la cadena de caracteres recibida en el archivo recibido y dependiendo si recibe el append en true o false sobreescribe o no el archivo
            /// </summary>
            /// <param name="data"></param>
            /// <param name="nombreArchivo"></param>
            /// <param name="Append"></param>
            /// <exception cref="FileManagerException"></exception>
           public static void Guardar(string data, string nombreArchivo, bool Append)
           {
               string filePath=Path.Combine(path, nombreArchivo);

               using (StreamWriter wr = new StreamWriter(filePath, Append))
               {
                 wr.WriteLine(data);
               }
          
           }


        /// <summary>
        /// Serializa el objeto recibido en un archivo Json 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elemento"></param>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        public static bool Serializar<T>(T elemento, string nombreArchivo) where T :class
        {
          string ruta = Path.Combine(path, nombreArchivo);
           string json = JsonSerializer.Serialize(elemento, new JsonSerializerOptions
           {
                    WriteIndented = true,
           });
           File.WriteAllText(ruta, json);
           return true;
        }

            /// <summary>
            /// Corrobora que el directorio exista de no ser asi lo crea
            /// </summary>
            /// <exception cref="FileManagerException"></exception>
            private static void ValidaExistenciaDeDirectorio()
            {
                try
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }catch(FileManagerException ex)
                {
                    Guardar(ex.Message + ex.StackTrace + ex.GetType(), "logs.txt", true);
                    //throw;
                } 
                 catch (Exception ex)
                {
                    throw new FileManagerException("Error al creal el directorio",ex);

                }
                
            }
     }
       
}
