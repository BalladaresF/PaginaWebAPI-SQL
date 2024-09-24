using AndresBalladares_Proyecto.Models;

namespace AndresBalladares_Proyecto.Services
{
    public interface IServicioDirecciones
    {
        public Task<List<Direccion>> Get();
        public Task<bool> Guardar(Direccion direccion);
        public Task<bool> Eliminar(int ID);
        public Task<bool> Actualizar(int ID, Direccion direccion);
    }
}
