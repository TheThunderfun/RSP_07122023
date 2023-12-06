using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;


namespace Entidades.Modelos
{
    public delegate void DelegadoNuevoIngreso(IComestible menu);
    public delegate void DelegadoDemoraAtencion(double demora);

    public class Cocinero<T> where T : IComestible,new()
    {
        private CancellationTokenSource cancellation;
        private int cantPedidosFinalizados;
        private double demoraPreparacionTotal;
        private T menu;
        private string nombre;
        private Task tarea;


        public event DelegadoDemoraAtencion OnDemora;
        public event DelegadoNuevoIngreso OnIngreso;


        public Cocinero(string nombre)
        {
            this.nombre = nombre;
        }

        //No hacer nada
        public bool HabilitarCocina
        {
            get
            {
                return this.tarea is not null && (this.tarea.Status == TaskStatus.Running ||
                    this.tarea.Status == TaskStatus.WaitingToRun ||
                    this.tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                if (value && !this.HabilitarCocina)
                {
                    this.cancellation = new CancellationTokenSource();
                    this.IniciarIngreso();
                }
                else
                {
                    this.cancellation.Cancel();
                }
            }
        }

        //no hacer nada
        public double TiempoMedioDePreparacion { get => this.cantPedidosFinalizados == 0 ? 0 : this.demoraPreparacionTotal / this.cantPedidosFinalizados; }
        public string Nombre { get => nombre; }
        public int CantPedidosFinalizados { get => cantPedidosFinalizados; }


        /// <summary>
        /// realiza el ingreso en un hilo secundario y dependiendo si la tarea fue cancelada realiza el llamado a NotificarNuevoIngreso y EsperarProximoIngreso, ademas de aumentar la cantidad de pedidos y guardar el ticket en la base de datos
        /// </summary>
        private void IniciarIngreso()
        {
            this.tarea=Task.Run(() => { 
 
                while (!this.cancellation.IsCancellationRequested)
                {
                    NotificarNuevoIngreso();
                    //Thread.Sleep(1000);
                    EsperarProximoIngreso();
                    this.cantPedidosFinalizados++;
                    DataBase.DataBaseManager.GuardarTicket(this.nombre, this.menu);
                    
                }         
                              
            },this.cancellation.Token);
           
        }
        /// <summary>
        /// realiza la creacion de un nuevo menu si OnIngreso no posee suscriptores
        /// </summary>
        private void NotificarNuevoIngreso()
        {
            if (this.OnIngreso != null)
            {   
               this.menu = new();
               this.menu.IniciarPreparacion();
               //this.menu.ToString();
               this.OnIngreso.Invoke(this.menu);
            }
        }
        /// <summary>
        /// si posee un suscriptor notificara los segundos transcurridos en la preparacion del pedido mientras no sea cancelada la tarea y el estado del menu sea falso
        /// </summary>
        private void EsperarProximoIngreso()
        {
            int tiempoEspera = 0;

            if (this.OnDemora != null)
            {
                while (this.menu.Estado==false && !this.cancellation.IsCancellationRequested)
                {
                    this.OnDemora.Invoke(tiempoEspera);
                    Thread.Sleep(1000);
                    tiempoEspera++;
                }
            }
            this.demoraPreparacionTotal += tiempoEspera;
            
        }
    }
}
