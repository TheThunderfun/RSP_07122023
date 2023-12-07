using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;


namespace Entidades.Modelos
{
    public delegate void DelegadoPedidoEnCurso(IComestible menu);
    public delegate void DelegadoDemoraAtencion(double demora);

    public class Cocinero<T> where T : IComestible,new()
    {
        private CancellationTokenSource cancellation;
        private int cantPedidosFinalizados;
        private double demoraPreparacionTotal;
        private T pedidoEnPreparacion;
        private Mozo<T> Mozo;
        private Queue<T> pedidos;
        private string nombre;
        private Task tarea;



        public event DelegadoDemoraAtencion OnDemora;
        public event DelegadoPedidoEnCurso OnPedido;



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
                    this.Mozo.EmpezarATrabajar =true;
                    this.EmpezarACocinar();
                }
                else
                {
                    this.cancellation.Cancel();
                    this.Mozo.EmpezarATrabajar=!this.Mozo.EmpezarATrabajar;
                }
            }
        }

        //no hacer nada
        public Cocinero(string nombre)
        {
            this.nombre = nombre;
            this.Mozo= new Mozo<T>();
            this.pedidos = new Queue<T>();
            this.Mozo.OnPedido += TomarPedidoNuevo;
            
        }
        public double TiempoMedioDePreparacion { get => this.cantPedidosFinalizados == 0 ? 0 : this.demoraPreparacionTotal / this.cantPedidosFinalizados; }
        public string Nombre { get => nombre; }
        public int CantPedidosFinalizados { get => cantPedidosFinalizados; }
        public Queue<T> Pedidos { get => pedidos; }
       

        /// <summary>
        /// realiza el ingreso en un hilo secundario y dependiendo si la tarea fue cancelada realiza el llamado a NotificarNuevoIngreso y EsperarProximoIngreso, ademas de aumentar la cantidad de pedidos y guardar el ticket en la base de datos
        /// </summary>
        private void EmpezarACocinar()
        {
            this.tarea=Task.Run(() => { 
 
                while (!this.cancellation.IsCancellationRequested )
                {
                    if(pedidos.Count>0)
                    {
                        pedidoEnPreparacion=this.pedidos.Dequeue();
                        this.OnPedido.Invoke(this.pedidoEnPreparacion);
                        EsperarProximoIngreso();
                        cantPedidosFinalizados++;
                        DataBase.DataBaseManager.GuardarTicket(this.Nombre,this.pedidoEnPreparacion);
                    }

                }         
                              
            },this.cancellation.Token);
           
        }

        private void TomarPedidoNuevo(T menu)
        {
            if(this.OnPedido != null)
            {
                this.pedidos.Enqueue(menu);
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
                while (this.pedidoEnPreparacion.Estado==false && !this.cancellation.IsCancellationRequested)
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
