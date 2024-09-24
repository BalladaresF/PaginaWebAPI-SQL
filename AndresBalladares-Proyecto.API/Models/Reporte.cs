using System.ComponentModel.DataAnnotations;

namespace AndresBalladares_Proyecto.API.Models
{
    public class Reporte
    {
        //Este modelo es utilizado para obtener los clientes con más pedidos por mes.
        [Key]
        public int IDCliente { get; set; }
        //public List<int> IDPedidos { get; set; }
        public int MontoTotalPedidos { get; set; }
    }
}
