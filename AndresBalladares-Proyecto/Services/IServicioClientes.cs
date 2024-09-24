using AndresBalladares_Proyecto.Models;

namespace AndresBalladares_Proyecto.Services
{
    public interface IServicioClientes
    {
        public Task<List<Cliente>> Get();
        public Task<bool> Guardar(Cliente cliente);
        public Task<bool> Eliminar(int ID);
        public Task<bool> Actualizar(int ID, Cliente cliente);
    }
}
