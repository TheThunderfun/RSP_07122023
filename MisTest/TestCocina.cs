using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Modelos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MisTest
{
    [TestClass]
    public class TestCocina
    {
        [TestMethod]
        [ExpectedException(typeof(FileManagerException))]
        public void AlGuardarUnArchivo_ConNombreInvalido_TengoUnaExcepcion()
        {
            //arrange
            string data = "hola";
            string nombreArchivo ="#¡?!?=¡";
            bool append = true;

            //act
           
            //assert
            FileManager.Guardar(data, nombreArchivo, append);
        }

        [TestMethod]

        public void AlInstanciarUnCocinero_SeEspera_PedidosCero()
        {
            //arrange
            int pedidos = 0;
            //act
            Cocinero<Hamburguesa> hamburguesero;
            hamburguesero = new Cocinero<Hamburguesa>("Esteban");

            //assert
            Assert.AreEqual(pedidos, hamburguesero.CantPedidosFinalizados);
        }
    }
}