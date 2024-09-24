using AndresBalladares_Proyecto.Models;
using Newtonsoft.Json;
using System.Text;

namespace AndresBalladares_Proyecto.Services
{
    public class ServicioDirecciones : IServicioDirecciones
    {
        private string _baseurl;

        public ServicioDirecciones()
        {
            //la URL puesta es la del API.
            _baseurl = "http://localhost:5040";
        }

        public async Task<List<Direccion>> Get()
        {
            List<Direccion> Lista = new List<Direccion>();
            var Cliente = new HttpClient();
            Cliente.BaseAddress = new Uri(_baseurl);
            var response = await Cliente.GetAsync("api/Direcciones/Buscar");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<Direccion>>(json_respuesta);
                Lista = resultado;
            }
            return Lista;
        }

        public async Task<bool> Guardar(Direccion direccion)
        {
            bool respuesta = false;
            var Cliente = new HttpClient();
            Cliente.BaseAddress = new Uri(_baseurl);

            var contenido = new StringContent(JsonConvert.SerializeObject(direccion), Encoding.UTF8, "application/json");
            var response = await Cliente.PostAsync($"api/Direcciones/Insertar", contenido);

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

            var response = await cliente.DeleteAsync($"api/Direcciones/Eliminar/{ID}");

            if (response.IsSuccessStatusCode)
            {
                respuesta = true;
            }
            return respuesta;
        }

        public async Task<bool> Actualizar(int ID, Direccion direccion)
        {
            bool Respuesta = false;
            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseurl);

            var contenido = new StringContent(JsonConvert.SerializeObject(direccion), Encoding.UTF8, "application/json");
            var response = await cliente.PutAsync($"api/Direcciones/Actualizar/{ID}", contenido);

            if (response.IsSuccessStatusCode)
            {
                Respuesta = true;
            }
            return Respuesta;
        }
    }
}
