using System.ComponentModel.DataAnnotations;

namespace AndresBalladares_Proyecto.API.Models
{
    public class Cliente
    {
        [Key]
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int DineroCompradoTotal { get; set; }
        public int DineroCompradoUltimoAño { get; set; }
        public int DineroCompradoUltimosSeisMeses { get; set; }
        public string Clave { get; set; } 
    }
}

//Cada cliente puede tener múltiples direcciones.
