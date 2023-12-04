using Entidades.Enumerados;


namespace Entidades.MetodosDeExtension
{
    public static class IngredientesExtension
    {
        public static double CalcularCostoIngredientes(this List<EIngrediente> ingredientes, int costoInicial)
        {
            double costoIncrementado = 0;
            foreach (EIngrediente e in ingredientes)
            {
                double costo = (int)e;
                costoIncrementado += costoInicial * (costo/100);
            }
            return costoIncrementado;
            
        }

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
