using AndresBalladares_Proyecto.Models;

namespace AndresBalladares_Proyecto.Services
{
    public interface IServicioPedidos
    {
        public Task<List<Pedido>> Get();
        public Task<bool> Guardar(Pedido pedido);
        public Task<bool> Eliminar(int ID);
        public Task<bool> Actualizar(int ID, Pedido pedido);
    }
}
