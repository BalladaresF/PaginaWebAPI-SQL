namespace AndresBalladares_Proyecto.Models
{
    public class Pedido
    {
        public int ID { get; set; }
        public int IDCliente { get; set; }
        public int IDInventario { get; set; }
        public int IDDireccion { get; set; }
        public int Cantidad { get; set; }
        public int CostoSinIva { get; set; }
        public int Costo { get; set; }//IVA: 13%
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } //Estados: en proceso, facturado, por entregar, entregado.
    }
}
