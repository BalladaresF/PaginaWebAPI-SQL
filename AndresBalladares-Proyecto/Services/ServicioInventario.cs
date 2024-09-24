using AndresBalladares_Proyecto.Models;
using Newtonsoft.Json;
using System.Text;

namespace AndresBalladares_Proyecto.Services
{
    public class ServicioInventario : IServicioInventario
    {
        private string _baseurl;

        public ServicioInventario()
        {
            //la URL puesta es la del API.
            _baseurl = "http://localhost:5040";
        }

        public async Task<List<Inventario>> Get()
        {
            List<Inventario> Lista = new List<Inventario>();
            var Cliente = new HttpClient();
            Cliente.BaseAddress = new Uri(_baseurl);
            var response = await Cliente.GetAsync("api/Inventario/Buscar");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<Inventario>>(json_respuesta);
                Lista = resultado;
            }
            return Lista;
        }

        public async Task<bool> Guardar(Inventario inventario)
        {
            bool respuesta = false;
            var Cliente = new HttpClient();
            Cliente.BaseAddress = new Uri(_baseurl);

            var contenido = new StringContent(JsonConvert.SerializeObject(inventario), Encoding.UTF8, "application/json");
            var response = await Cliente.PostAsync($"api/Inventario/Insertar", contenido);

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

            var response = await cliente.DeleteAsync($"api/Inventario/Eliminar/{ID}");

            if (response.IsSuccessStatusCode)
            {
                respuesta = true;
            }
            return respuesta;
        }

        public async Task<bool> Actualizar(int ID, Inventario inventario)
        {
            bool Respuesta = false;
            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseurl);

            var contenido = new StringContent(JsonConvert.SerializeObject(inventario), Encoding.UTF8, "application/json");
            var response = await cliente.PutAsync($"api/Inventario/Actualizar/{ID}", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;
            }
            return Respuesta;
        }
    }
}
