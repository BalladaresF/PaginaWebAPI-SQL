using AndresBalladares_Proyecto.Models;

namespace AndresBalladares_Proyecto.Services
{
    public interface IServicioReportes
    {
        public Task<List<Pedido>> PedidosDia(DateTime fecha);
        public Task<List<Pedido>> PedidosUltimoMes();
        public Task<List<Pedido>> PedidosUltimoTrimestre();
        public Task<List<Pedido>> PedidosBodega(int IDBodega);
        public Task<List<Pedido>> PedidosClienteDia(int IDCliente, DateTime fecha);
        public Task<List<Pedido>> PedidosClienteMes(int IDCliente);
        public Task<List<Pedido>> PedidosClienteHoraMinutos(int IDCliente, int hora, int minuto);
        public Task<List<Reporte>> ClientesMasPedidosMes(int mes, int anio);
    }
}
