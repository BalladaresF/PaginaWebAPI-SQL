namespace AndresBalladares_Proyecto.Models
{
    public class Inventario
    {
        public int ID { get; set; }
        public int Cantidad { get; set; }
        public int IDBodega { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Tipo { get; set; } //tipos:  cerveza, tequila, ron, ginebra, wiskey, digestivo, agua ardiente, vino tinto, vino blanco, vino rosado, champagne.
        public int Precio { get; set; }
    }
}
