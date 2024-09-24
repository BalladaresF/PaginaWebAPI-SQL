namespace AndresBalladares_Proyecto.Models
{
    public class Direccion
    {
        public int ID { get; set; }
        public int IDCliente { get; set; }
        public string Provincia { get; set; }
        public string Canton { get; set; }
        public string Distrito { get; set; }
        public string PuntoWaze { get; set; }
        public string URL { get; set; }
        public bool EsCondominio { get; set; }
    }
}
