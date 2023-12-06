using Entidades.Enumerados;


namespace Entidades.MetodosDeExtension
{
    public static class IngredientesExtension
    {
        /// <summary>
        ///  toma el costo inicial e incrementa su valor porcentualmente en base a los valores de la lista de Ingredientes. 
        /// </summary>
        /// <param name="ingredientes"></param>
        /// <param name="costoInicial"></param>
        /// <returns></returns>
        public static double CalcularCostoIngredientes(this List<EIngrediente> ingredientes, int costoInicial)
        {
            double costoIncrementado = costoInicial;
            foreach (EIngrediente e in ingredientes)
            {
                double costo = (int)e;
                costoIncrementado += costoInicial * (costo/100);
            }
            return costoIncrementado;
            
        }

        /// <summary>
        /// Genera una lista de ingredientes de manera aleatoria
        /// </summary>
        /// <param name="rand"></param>
        /// <returns></returns>
        public static List<EIngrediente> IngredientesAleatorios(this Random rand)
        {

            List<EIngrediente> ingredientes = new List<EIngrediente>()
            {
                EIngrediente.ADHERESO,
                EIngrediente.QUESO,
                EIngrediente.JAMON,
                EIngrediente.HUEVO,
                EIngrediente.PANCETA
            };

            int tamLista = ingredientes.Count;
            int nIngredientes= rand.Next(1, tamLista +1);

            return ingredientes.Take(nIngredientes).ToList();
        }



    }
}
