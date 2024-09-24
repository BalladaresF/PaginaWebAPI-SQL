using AndresBalladares_Proyecto.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace AndresBalladares_Proyecto.Services
{
    public class ServicioReportes : IServicioReportes
    {
        private string _baseurl;
        private readonly HttpClient _httpClient;

        public ServicioReportes()
        {
            //la URL puesta es la del API. En este caso, se ha añadido api/Reportes por comodidad.
            _baseurl = "http://localhost:5040/api/Reportes";
            _httpClient = new HttpClient();
        }

        //Este método se encarga de obtener todas las listas.
        private async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync($"{_baseurl}/{endpoint}");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonResponse);
            }
            else
            {
                throw new HttpRequestException($"Error al obtener datos del endpoint {endpoint}");
            }
        }

        public async Task<List<Pedido>> PedidosDia(DateTime fecha)
        {
            return await GetAsync<List<Pedido>>($"PedidosPorFecha/{fecha.ToString("yyyy-MM-dd")}");
        }

        public async Task<List<Pedido>> PedidosUltimoMes()
        {
            return await GetAsync<List<Pedido>>("PedidosUltimoMes");
        }

        public async Task<List<Pedido>> PedidosUltimoTrimestre()
        {
            return await GetAsync<List<Pedido>>("PedidosUltimoTrimestre");
        }

        public async Task<List<Pedido>> PedidosBodega(int IDBodega)
        {
            return await GetAsync<List<Pedido>>($"PedidosPorBodega/{IDBodega}");
        }

        public async Task<List<Pedido>> PedidosClienteDia(int IDCliente, DateTime fecha)
        {
            return await GetAsync<List<Pedido>>($"PedidosPorClienteYDia/{IDCliente}/{fecha.ToString("yyyy-MM-dd")}");
        }

        public async Task<List<Pedido>> PedidosClienteMes(int IDCliente)
        {
            return await GetAsync<List<Pedido>>($"PedidosPorClienteUltimoMes/{IDCliente}");
        }

        public async Task<List<Pedido>> PedidosClienteHoraMinutos(int IDCliente, int hora, int minuto)
        {
            return await GetAsync<List<Pedido>>($"PedidosPorClienteEnHoraMinuto/{IDCliente}/{hora}/{minuto}");
        }

        public async Task<List<Reporte>> ClientesMasPedidosMes(int mes, int anio)
        {
            return await GetAsync<List<Reporte>>($"PedidosPorClienteEnMesAnio/{mes}/{anio}");
        }
    }
}
