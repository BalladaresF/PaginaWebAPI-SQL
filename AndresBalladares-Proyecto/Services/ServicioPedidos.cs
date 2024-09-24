using AndresBalladares_Proyecto.Models;
using Newtonsoft.Json;
using System.Text;

namespace AndresBalladares_Proyecto.Services
{
    public class ServicioPedidos : IServicioPedidos
    {
        private string _baseurl;

        public ServicioPedidos()
        {
            //la URL puesta es la del API.
            _baseurl = "http://localhost:5040";
        }

        public async Task<List<Pedido>> Get()
        {
            List<Pedido> Lista = new List<Pedido>();
            var Cliente = new HttpClient();
            Cliente.BaseAddress = new Uri(_baseurl);
            var response = await Cliente.GetAsync("api/Pedidos/Buscar");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<Pedido>>(json_respuesta);
                Lista = resultado;
            }
            return Lista;
        }

        public async Task<bool> Guardar(Pedido pedido)
        {
            bool respuesta = false;
            var Cliente = new HttpClient();
            Cliente.BaseAddress = new Uri(_baseurl);

            var contenido = new StringContent(JsonConvert.SerializeObject(pedido), Encoding.UTF8, "application/json");
            var response = await Cliente.PostAsync($"api/Pedidos/Insertar", contenido);

            if (response.IsSuccessStatusCode)
            {
                respuesta = true;
            }
            return respuesta;
        }

        public async Task<bool> Eliminar(int ID)
        {
            bool respuesta = false;
            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseurl);

            var response = await cliente.DeleteAsync($"api/Pedidos/Eliminar/{ID}");

            if (response.IsSuccessStatusCode)
            {
                respuesta = true;
            }
            return respuesta;
        }

        public async Task<bool> Actualizar(int ID, Pedido pedido)
        {
            bool Respuesta = false;
            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseurl);

            var contenido = new StringContent(JsonConvert.SerializeObject(pedido), Encoding.UTF8, "application/json");
            var response = await cliente.PutAsync($"api/Pedidos/Actualizar/{ID}", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;
            }
            return Respuesta;
        }
    }
}
