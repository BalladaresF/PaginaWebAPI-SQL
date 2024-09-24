namespace AndresBalladares_Proyecto.Models
{
    public class Reporte
    {
        //Este modelo es utilizado para obtener los clientes con más pedidos por mes.
        public int IDCliente { get; set; }
        //public List<int> IDPedidos { get; set; }
        public int MontoTotalPedidos { get; set; }
    }
}
