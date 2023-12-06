using Entidades.Enumerados;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;
using Entidades.MetodosDeExtension;
using System.Text;
using Entidades.DataBase;

namespace Entidades.Modelos
{
    public class Hamburguesa: IComestible
    {

        private double costo;
        private static int costoBase;
        private bool esDoble;
        private bool estado;
        private string imagen;
        List<EIngrediente> ingredientes;
        Random random;

        public bool Estado { get => estado; }
        public string Imagen { get => imagen; }
        public string Ticket => $"{this}\nTotal a pagar:{this.costo}";
        
        /// <summary>
        /// Agrega los ingredientes a la hamburguesa
        /// </summary>
        private void AgregarIngredientes()
        {
            this.ingredientes = this.random.IngredientesAleatorios();
        }

        /// <summary>
        /// Calcula el costo de la hamburguesa dependiendo la cantidad de ingredientes que tenga y cambia el estado del "pedido"
        /// </summary>
        /// <param name="cocinero"></param>
        public void FinalizarPreparacion(string cocinero)
        {
            this.costo = ingredientes.CalcularCostoIngredientes(costoBase);
            this.estado = !this.Estado;
        }
        static Hamburguesa() => Hamburguesa.costoBase = 1500;
        public  Hamburguesa() : this(false) { }
        public Hamburguesa(bool esDoble)
        {
            this.esDoble = esDoble;
            this.random = new Random();
        }

        /// <summary>
        /// Si el estado es false crea un numero random y en base al numero busca la imagen en la base de datos se la asigna a la hamburguesa y agrega los ingredientes a la hamburguesa
        /// </summary>
        public void IniciarPreparacion()
        {
            if (!this.Estado)
            {   
                string imgStr= "Hamburguesa_" + this.random.Next(1, 9);
                this.imagen=DataBaseManager.GetImagenComida(imgStr);
                this.AgregarIngredientes();
            }
        }


        /// <summary>
        /// muestra los datos de la hamburguesa
        /// </summary>
        /// <returns></returns>
        private string MostrarDatos()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Hamburguesa {(this.esDoble ? "Doble" : "Simple")}");
            stringBuilder.AppendLine("Ingredientes: ");
            this.ingredientes.ForEach(i => stringBuilder.AppendLine(i.ToString()));
            return stringBuilder.ToString();

        }

        public override string ToString() => this.MostrarDatos();


    }
}