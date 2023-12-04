namespace Entidades.Interfaces
{
    public interface IComestible
    {
        bool Estado { get; }
        string Imagen { get; }
        string Ticket { get; }

        public void FinalizarPreparacion(string cocinero);
        public void IniciarPreparacion();
    }
}
