using AndresBalladares_Proyecto.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AndresBalladares_Proyecto.API.Controllers
{
    [Route("api/Reportes")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly Memory Datos;
        public ReportesController(Context context)
        {
            Datos = new Memory(context);
        }

        //Manejo de pedidos por día:
        //<El formato de la fecha debe ser Año-Mes-Día.
        [HttpGet("PedidosPorFecha/{fecha}")]
        public IActionResult GetPedidosPorFecha(DateTime fecha)
        {
            var pedidosPorFecha = Datos.GetPedidosPorFecha(fecha);
            return Ok(pedidosPorFecha);
        }

        //Manejo de pedidos del último mes:
        [HttpGet("PedidosUltimoMes")]
        public IActionResult GetPedidosUltimoMes()
        {
            var fechaInicio = DateTime.Now.AddMonths(-1);
            var fechaFin = DateTime.Now;

            var pedidosUltimoMes = Datos.GetPedidosEnRango(fechaInicio, fechaFin);
            return Ok(pedidosUltimoMes);
        }

        //Manejo de pedidos trimestrales:
        [HttpGet("PedidosUltimoTrimestre")]
        public IActionResult GetPedidosUltimoTrimestre()
        {
            var fechaInicio = DateTime.Now.AddMonths(-3);
            var fechaFin = DateTime.Now;

            var pedidosUltimoTrimestre = Datos.GetPedidosEnRango(fechaInicio, fechaFin);
            return Ok(pedidosUltimoTrimestre);
        }

        //Manejo de pedidos por bodega:
        [HttpGet("PedidosPorBodega/{IDBodega}")]
        public IActionResult GetPedidosPorBodega(int IDBodega)
        {
            if (Datos.BodegaExiste(IDBodega))
            {
                var pedidosPorBodega = Datos.GetPedidosPorBodega(IDBodega);
                return Ok(pedidosPorBodega);
            }
            else
            {
                return BadRequest("La solicitud es incorrecta. La bodega no existe.");
            }
        }

        //Manejo de pedidos por cliente por día:
        [HttpGet("PedidosPorClienteYDia/{IDCliente}/{fecha}")]
        public IActionResult GetPedidosPorClienteYDia(int IDCliente, DateTime fecha)
        {
            if (Datos.ClienteExiste(IDCliente))
            {
                var pedidos = Datos.GetPedidosPorClienteYDia(IDCliente, fecha);
                return Ok(pedidos);
            }
            else
            {
                return BadRequest("La solicitud es incorrecta. El cliente no existe.");
            }
        }

        //Manejo de pedidos por cliente por mes:
        [HttpGet("PedidosPorClienteUltimoMes/{IDCliente}")]
        public IActionResult GetPedidosPorClienteUltimoMes(int IDCliente)
        {
            if (Datos.ClienteExiste(IDCliente))
            {
                var fechaInicio = DateTime.Now.AddMonths(-1);
                var fechaFin = DateTime.Now;

                var pedidosUltimoMes = Datos.GetPedidosEnRangoPorCliente(IDCliente, fechaInicio, fechaFin);
                return Ok(pedidosUltimoMes);
            }
            else
            {
                return BadRequest("La solicitud es incorrecta. El cliente no existe.");
            }
        }

        //Manejo de pedidos por cliente por horas y minutos:
        [HttpGet("PedidosPorClienteEnHoraMinuto/{IDCliente}/{hora}/{minuto}")]
        public IActionResult GetPedidosPorClienteEnHoraMinuto(int IDCliente, int hora, int minuto)
        {
            if (Datos.ClienteExiste(IDCliente))
            {
                if (Datos.HoraYMinutoEsValido(hora, minuto))
                {
                    var pedidosEnHoraMinuto = Datos.GetPedidosPorClienteEnHoraMinuto(IDCliente, hora, minuto);
                    return Ok(pedidosEnHoraMinuto);
                }
                else
                {
                    return BadRequest("La solicitud es incorrecta. Los valores de hora y minuto no son válidos.");
                }
            }
            else
            {
                return BadRequest("La solicitud es incorrecta. El cliente no existe.");
            }
        }

        //Manejo de clientes que hacen más pedidos en un mes específico
        [HttpGet("PedidosPorClienteEnMesAnio/{mes}/{anio}")]
        public IActionResult PedidosPorClienteEnMesAño(int mes, int anio)
        {
            if (Datos.MesEsValido(mes))
            {
                var pedidosUltimoMes = Datos.GetClientesConMasPedidosEnMes(mes, anio);
                return Ok(pedidosUltimoMes);
            }
            else
            {
                return BadRequest("La solicitud es incorrecta. El valor de mes no es válido.");
            }
            
        }
    }
}
