using AndresBalladares_Proyecto.API.Data;
using AndresBalladares_Proyecto.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AndresBalladares_Proyecto.API.Controllers
{
    [Route("api/Pedidos")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly Memory Datos;
        public PedidosController(Context context)
        {
            Datos = new Memory(context);
            if (Datos.PedidosVacio())
            {
                Pedido PedidoInicial = new Pedido
                {
                    ID = 100,
                    IDCliente = 118520229,
                    IDInventario = 100,
                    IDDireccion = 100,
                    Cantidad = 2,
                    CostoSinIva = 10000,
                    Costo = 11300,
                    Fecha = new DateTime(2024, 07, 16, 0, 0, 0),
                    Estado = "Entregado",
                };
                Datos.AgregarPedido(PedidoInicial);
            }
        }

        //Manejo de Create:
        [HttpPost("Insertar")]
        public ActionResult Create([FromBody] Pedido value)
        {
            var DireccionExistente = Datos.GetListaDirecciones().Find(p => p.ID == value.IDDireccion);
            if (Datos.ClienteExiste(value.IDCliente))
            {
                if (Datos.InventarioExiste(value.IDInventario))
                {
                    if (Datos.DireccionExiste(value.IDDireccion) && DireccionExistente.IDCliente == value.IDCliente)
                    {
                        Datos.AgregarPedido(value);
                        return Ok(value);
                    }
                    else
                    {
                        return BadRequest("La solicitud es incorrecta. El usuario no posee el ID de dirección escrito.");
                    }
                    
                }
                else
                {
                    return BadRequest("La solicitud es incorrecta. El ID del inventario no existe.");
                }
            }
            else
            {
                return BadRequest("La solicitud es incorrecta. El ID del cliente no existe.");
            }
            
        }

        //Manejo de Read:
        [HttpGet("Buscar")]
        public ActionResult<IEnumerable<string>> Get()
        {
            List<API.Models.Pedido> Lista = new List<Pedido>();
            Lista = Datos.GetListaPedidos();
            return Ok(Lista);
        }

        //Manejo de Delete (por ID):
        [HttpDelete("Eliminar/{ID}")]
        public IActionResult Delete(int ID)
        {
            if (Datos.PedidoExiste(ID))

            {
                Datos.EliminarPedido(ID);
                return Ok(ID);
            }
            else
            {
                return BadRequest("La solicitud es incorrecta. El ID no existe.");
            }
        }

        //Manejo de Edit (por ID)
        [HttpPut("Actualizar/{ID}")]
        public IActionResult Edit(int ID, [FromBody] Pedido value)
        {
            var PedidoExistente = Datos.GetListaPedidos().Find(p => p.ID == ID);
            var DireccionExistente = Datos.GetListaDirecciones().Find(p => p.ID == value.IDDireccion);
            if (Datos.PedidoExiste(ID))
            {
                if (Datos.PedidoExiste(value.ID) && PedidoExistente.ID != value.ID)
                {
                    return BadRequest("La solicitud es incorrecta. El ID ingresado pertenece a otro pedido.");
                }
                else
                {
                    if (Datos.ClienteExiste(value.IDCliente))
                    {
                        if (Datos.InventarioExiste(value.IDInventario))
                        {
                            if (Datos.DireccionExiste(value.IDDireccion) && DireccionExistente.IDCliente == value.IDCliente)
                            {
                                Datos.ActualizarPedido(ID, value);
                                return Ok(value);
                            }
                            else
                            {
                                return BadRequest("La solicitud es incorrecta. El usuario no posee el ID de dirección escrito.");
                            }
                            
                        }
                        else
                        {
                            return BadRequest("La solicitud es incorrecta. El ID del inventario no existe.");
                        }
                    }
                    else
                    {
                        return BadRequest("La solicitud es incorrecta. El ID del cliente no existe.");
                    }
                }
            }
            else
            {
                return BadRequest("La solicitud es incorrecta. El ID no existe.");
            }
        }
    }
}
