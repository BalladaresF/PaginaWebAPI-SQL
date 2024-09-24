using System.ComponentModel.DataAnnotations;

namespace AndresBalladares_Proyecto.API.Models
{
    public class Direccion
    {
        [Key]
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
