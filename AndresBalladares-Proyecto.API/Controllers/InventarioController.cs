using AndresBalladares_Proyecto.API.Data;
using AndresBalladares_Proyecto.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AndresBalladares_Proyecto.API.Controllers
{
    [Route("api/Inventario")]
    [ApiController]
    public class InventarioController : ControllerBase
    {
        private readonly Memory Datos;
        public InventarioController (Context context)
        {
            Datos = new Memory(context);
            if (Datos.InventarioVacio())
            {
                Inventario InventarioInicial = new Inventario
                {
                    ID = 100,
                    Cantidad = 50,
                    IDBodega = 100,
                    FechaIngreso = new DateTime(2023, 12, 01, 0, 0, 0),
                    FechaVencimiento = new DateTime(2025, 12, 01, 0, 0, 0),
                    Tipo = "Cerveza",
                    Precio = 5000,
                };
                Datos.AgregarInventario(InventarioInicial);
            }
        }

        //Manejo de Create:
        [HttpPost("Insertar")]
        public ActionResult Create([FromBody] Inventario value)
        {
            if (!Datos.InventarioExiste(value.ID))
            {
                Datos.AgregarInventario(value);
                return Ok(value);
            }
            else
            {
                return BadRequest("La solicitud es incorrecta. El ID ya existe.");
            }
        }

        //Manejo de Read:
        [HttpGet("Buscar")]
        public ActionResult<IEnumerable<string>> Get()
        {
            List<API.Models.Inventario> Lista = new List<Inventario>();
            Lista = Datos.GetListaInventario();
            return Ok(Lista);
        }

        //Manejo de Delete (por ID):
        [HttpDelete("Eliminar/{ID}")]
        public IActionResult Delete(int ID)
        {
            if (Datos.InventarioExiste(ID))
            {
                Datos.EliminarInventario(ID);
                return Ok(ID);
            }
            else
            {
                return BadRequest("La solicitud es incorrecta. El ID no existe.");
            }
        }

        //Manejo de Edit (por ID)
        [HttpPut("Actualizar/{ID}")]
        public IActionResult Edit(int ID, [FromBody] Inventario value)
        {
            var InventarioExistente = Datos.GetListaInventario().Find(p => p.ID == ID);
            if (Datos.InventarioExiste(ID))
            {
                if (Datos.InventarioExiste(value.ID) && InventarioExistente.ID != value.ID)
                {
                    return BadRequest("La solicitud es incorrecta. El ID ingresado pertenece a otro inventario.");
                }
                else
                {
                    Datos.ActualizarInventario(ID, value);
                    return Ok(value);
                }
            }
            else
            {
                return BadRequest("La solicitud es incorrecta. El ID no existe.");
            }
        }
    }
}
