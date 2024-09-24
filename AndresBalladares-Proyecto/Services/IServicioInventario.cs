using AndresBalladares_Proyecto.Models;

namespace AndresBalladares_Proyecto.Services
{
    public interface IServicioInventario
    {
        public Task<List<Inventario>> Get();
        public Task<bool> Guardar(Inventario inventario);
        public Task<bool> Eliminar(int ID);
        public Task<bool> Actualizar(int ID, Inventario inventario);
    }
}
