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
            string ruta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            path = Path.Combine(ruta, "\\20230712_FranciscoHulej\\");

            ValidaExistenciaDeDirectorio();

        }
           public static void Guardar(string data, string nombreArchivo, bool Append)
           {
                string filePath=Path.Combine(path, nombreArchivo);
                try
                {
                    using (StreamWriter wr = new StreamWriter(filePath, Append))//check
                    {
                        wr.WriteLine(data);
                    }
                }
                 catch(Exception ex) {
                    throw new FileManagerException("Error al guardar el archivo",ex);
                }
           }

            public static bool Serializar<T>(T elemento, string nombreArchivo)
            {
                return true;
            }

            private static void ValidaExistenciaDeDirectorio()
            {
                try
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                } catch (Exception e)
                {
                    throw new FileManagerException("Error al creal el directorio",e);
                }
            }
     }
       
}
