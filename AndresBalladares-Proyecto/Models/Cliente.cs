namespace AndresBalladares_Proyecto.Models
{
    public class Cliente
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int DineroCompradoTotal { get; set; }
        public int DineroCompradoUltimoAño { get; set; }
        public int DineroCompradoUltimosSeisMeses { get; set; }
        public string Clave { get; set; } //Contraseña de los usuarios al iniciar sesión.
    }
}
