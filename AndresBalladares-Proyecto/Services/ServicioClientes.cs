using AndresBalladares_Proyecto.Models;
using Newtonsoft.Json;
using System.Text;

namespace AndresBalladares_Proyecto.Services
{
    public class ServicioClientes : IServicioClientes
    {
        private string _baseurl;

        public ServicioClientes() 
        {
            //la URL puesta es la del API.
            _baseurl = "http://localhost:5040";
        }

        public async Task<List<Cliente>> Get()
        {
            List <Cliente> Lista = new List<Cliente>();
            var Cliente = new HttpClient();
            Cliente.BaseAddress = new Uri(_baseurl);
            var response = await Cliente.GetAsync("api/Clientes/Buscar");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<Cliente>>(json_respuesta);
                Lista = resultado;
            }
            return Lista; 
        }

        public async Task<bool> Guardar(Cliente obj_cliente)
        {
            bool respuesta = false;
            var Cliente = new HttpClient();
            Cliente.BaseAddress = new Uri(_baseurl);

            var contenido = new StringContent(JsonConvert.SerializeObject(obj_cliente), Encoding.UTF8, "application/json");
            var response = await Cliente.PostAsync($"api/Clientes/Insertar", contenido);

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

            var response = await cliente.DeleteAsync($"api/Clientes/Eliminar/{ID}");

            if (response.IsSuccessStatusCode)
            {
                respuesta = true;
            }
            return respuesta;
        }

        public async Task<bool> Actualizar(int ID, Cliente obj_cliente)
        {
            bool Respuesta = false;
            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseurl);

            var contenido = new StringContent(JsonConvert.SerializeObject(obj_cliente), Encoding.UTF8, "application/json");
            var response = await cliente.PutAsync($"api/Clientes/Actualizar/{ID}", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;
            }
            return Respuesta;
        }
    }
}
