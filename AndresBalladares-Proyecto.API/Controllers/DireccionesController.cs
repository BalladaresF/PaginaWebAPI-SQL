using AndresBalladares_Proyecto.API.Data;
using AndresBalladares_Proyecto.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AndresBalladares_Proyecto.API.Controllers
{
    [Route("api/Direcciones")]
    [ApiController]
    public class DireccionesController : ControllerBase
    {
        private readonly Memory Datos;
        public DireccionesController(Context context)
        {
            Datos = new Memory(context);
            if (Datos.DireccionesVacio())
            {
                Direccion ClienteInicial = new Direccion
                {
                    ID = 100,
                    IDCliente = 118520229,
                    Provincia = "San José",
                    Canton = "Moravia",
                    Distrito = "San Vicente",
                    PuntoWaze = "123321",
                    URL = "prueba.com",
                    EsCondominio = false
                };
                Datos.AgregarDireccion(ClienteInicial);
            }
        }

        //Manejo de Create:
        [HttpPost("Insertar")]
        public ActionResult Create([FromBody] Direccion value)
        {
            if (!Datos.DireccionExiste(value.ID))
            {
                if (Datos.ClienteExiste(value.IDCliente))
                {
                    Datos.AgregarDireccion(value);
                    return Ok(value);
                }
                else
                {
                    return BadRequest("La solicitud es incorrecta. El ID del cliente no existe.");
                }
                
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
            List<API.Models.Direccion> Lista = new List<Direccion>();
            Lista = Datos.GetListaDirecciones();
            return Ok(Lista);
        }

        //Manejo de Delete (por ID):
        [HttpDelete("Eliminar/{ID}")]
        public IActionResult Delete(int ID)
        {
            if (Datos.DireccionExiste(ID))
            {
                Datos.EliminarDireccion(ID);
                return Ok(ID);
            }
            else
            {
                return BadRequest("La solicitud es incorrecta. El ID no existe.");
            }
        }

        //Manejo de Edit (por ID)
        [HttpPut("Actualizar/{ID}")]
        public IActionResult Edit(int ID, [FromBody] Direccion value)
        {
            var DireccionExistente = Datos.GetListaDirecciones().Find(p => p.ID == ID);
            if (Datos.DireccionExiste(ID))
            {
                if (Datos.DireccionExiste(value.ID) && DireccionExistente.ID != value.ID)
                {
                    return BadRequest("La solicitud es incorrecta. El ID ingresado pertenece a otro usuario.");
                }
                else
                {
                    if (Datos.ClienteExiste(value.IDCliente))
                    {
                        Datos.ActualizarDireccion(ID, value);
                        return Ok(value);
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
