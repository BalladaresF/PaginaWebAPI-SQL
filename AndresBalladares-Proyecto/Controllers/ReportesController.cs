using AndresBalladares_Proyecto.Models;
using Microsoft.AspNetCore.Mvc;
using AndresBalladares_Proyecto.Services;
using Newtonsoft.Json;

namespace AndresBalladares_Proyecto.Controllers
{
    public class ReportesController : Controller
    {
        private readonly IServicioReportes _servicioReportes;

        public ReportesController(IServicioReportes servicioReportes)
        {
            _servicioReportes = servicioReportes;
        }

        [HttpGet]
        public IActionResult PedidosDia()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PedidosDia(DateTime fecha)
        {
            try
            {
                var pedidos = await _servicioReportes.PedidosDia(fecha);
                TempData["Pedidos"] = JsonConvert.SerializeObject(pedidos);

                return RedirectToAction("PedidosDia");
            }
            catch (Exception ex)
            {
                TempData["Errores"] = "No se han encontrado pedidos.";
                return RedirectToAction("PedidosDia");
            }
        }

        [HttpGet]
        public async Task<IActionResult> PedidosUltimoMes()
        {
            var pedidos = await _servicioReportes.PedidosUltimoMes();
            return View(pedidos);
        }

        [HttpGet]
        public async Task<IActionResult> PedidosUltimoTrimestre()
        {
            var pedidos = await _servicioReportes.PedidosUltimoTrimestre();
            return View(pedidos);
        }

        [HttpGet]
        public IActionResult PedidosBodega()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PedidosBodega(int IDBodega)
        {
            try
            {
                var pedidos = await _servicioReportes.PedidosBodega(IDBodega);
                TempData["Pedidos"] = JsonConvert.SerializeObject(pedidos);

                return RedirectToAction("PedidosBodega");
            }
            catch (Exception ex) {
                TempData["Errores"] = "ID de bodega no encontrado.";
                return RedirectToAction("PedidosBodega");
            }
        }

        [HttpGet]
        public IActionResult PedidosClienteDia()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PedidosClienteDia(int IDCliente, DateTime fecha)
        {
            try
            {
                var pedidos = await _servicioReportes.PedidosClienteDia(IDCliente, fecha);
                TempData["Pedidos"] = JsonConvert.SerializeObject(pedidos);

                return RedirectToAction("PedidosClienteDia");
            }
            catch (Exception ex)
            {
                TempData["Errores"] = "No se han encontrado pedidos.";
                return RedirectToAction("PedidosClienteDia");
            }
        }

        [HttpGet]
        public IActionResult PedidosClienteMes()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PedidosClienteMes(int IDCliente)
        {
            try
            {
                var pedidos = await _servicioReportes.PedidosClienteMes(IDCliente);
                TempData["Pedidos"] = JsonConvert.SerializeObject(pedidos);

                return RedirectToAction("PedidosClienteMes");
            }
            catch (Exception ex)
            {
                TempData["Errores"] = "No se han encontrado pedidos.";
                return RedirectToAction("PedidosClienteMes");
            }
        }

        [HttpGet]
        public IActionResult PedidosClienteHoraMinutos()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PedidosClienteHoraMinutos(int IDCliente, int hora, int minuto)
        {
            try
            {
                var pedidos = await _servicioReportes.PedidosClienteHoraMinutos(IDCliente, hora, minuto);
                TempData["Pedidos"] = JsonConvert.SerializeObject(pedidos);

                return RedirectToAction("PedidosClienteHoraMinutos");
            }
            catch (Exception ex)
            {
                TempData["Errores"] = "No se han encontrado pedidos.";
                return RedirectToAction("PedidosClienteHoraMinutos");
            }
        }

        [HttpGet]
        public IActionResult ClientesMasPedidosMes()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ClientesMasPedidosMes(int mes, int anio)
        {
            try
            {
                var clientes = await _servicioReportes.ClientesMasPedidosMes(mes, anio);
                TempData["Pedidos"] = JsonConvert.SerializeObject(clientes);

                return RedirectToAction("ClientesMasPedidosMes");
            }
            catch (Exception ex)
            {
                TempData["Errores"] = "No se han encontrado pedidos.";
                return RedirectToAction("ClientesMasPedidosMes");
            }
        }
    }
}
