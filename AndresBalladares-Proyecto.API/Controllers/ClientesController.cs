using AndresBalladares_Proyecto.API.Data;
using AndresBalladares_Proyecto.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Experimental.ProjectCache;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;

namespace AndresBalladares_Proyecto.API.Controllers
{
    [Route("api/Clientes")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly Memory Datos;
        public ClientesController (Context context)
        {
            Datos = new Memory(context);
            if (Datos.ClientesVacio())
            {
                Cliente ClienteInicial = new Cliente
                {
                    ID = 118520229,
                    Nombre = "Andrés Eduardo",
                    Apellidos = "Balladares Flores",
                    DineroCompradoTotal = 1000000,
                    DineroCompradoUltimoAño = 500000,
                    DineroCompradoUltimosSeisMeses = 100000,
                    Clave = "118520229AnB"
                };
                Datos.AgregarCliente(ClienteInicial);
            }
        }

        // Manejo de Create:
        [HttpPost("Insertar")]
        public async Task<ActionResult> Create([FromBody] Cliente value)
        {
            if (!Datos.ClienteExiste(value.ID))
            {
                Datos.AgregarCliente(value);
                return Ok(value);
            }
            else
            {
                return BadRequest("La solicitud es incorrecta. El ID ya existe.");
            }
        }

        // Manejo de Read:
        [HttpGet("Buscar")]
        public ActionResult<IEnumerable<string>> Get()
        {
            List<Cliente> Lista = new List<Cliente>();
            Lista = Datos.GetListaClientes();
            return Ok(Lista);
        }

        // Manejo de Delete (por ID):
        [HttpDelete("Eliminar/{ID}")]
        public async Task<IActionResult> Delete(int ID)
        {
            if (Datos.ClienteExiste(ID))
            {
                Datos.EliminarCliente(ID);
                return Ok(ID);
            }
            else
            {
                return BadRequest("La solicitud es incorrecta. El ID no existe.");
            }
        }

        // Manejo de Edit (por ID)
        [HttpPut("Actualizar/{ID}")]
        public async Task<IActionResult> Edit(int ID, [FromBody] Cliente value)
        {
            var clienteExistente = (Datos.GetListaClientes()).Find(p => p.ID == ID);
            if (Datos.ClienteExiste(ID))
            {
                if (Datos.ClienteExiste(value.ID) && clienteExistente.ID != value.ID)
                {
                    return BadRequest("La solicitud es incorrecta. El ID ingresado pertenece a otro usuario.");
                }
                else
                {
                    try
                    {
                        Datos.ActualizarCliente(ID, value);
                        return Ok(value);
                    }catch (Exception ex)
                    {
                        throw new Exception("Error actualizando cliente: " + ex.Message);
                    }
                    
                }
            }
            else
            {
                return BadRequest("La solicitud es incorrecta. El ID no existe.");
            }
        }

        //Código utilizando cache en lugar de una DB:
        //Manejo de Create:
        /*[HttpPost("Insertar")]
        public ActionResult Create([FromBody] Cliente value)
        {
            Memoria datos = new Memoria(Cache);
            if (!datos.ClienteExiste(value.ID))
            {
                datos.AgregarCliente(value);
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
            List<API.Models.Cliente> Lista = new List<Cliente>();
            Memoria datos = new Memoria(Cache);
            Lista = datos.GetListaClientes();
            return Ok(Lista);
        }

        //Manejo de Delete (por ID):
        [HttpDelete("Eliminar/{ID}")]
        public IActionResult Delete(int ID)
        {
            Memoria datos = new Memoria(Cache);
            if (datos.ClienteExiste(ID))
            {
                datos.EliminarCliente(ID);
                return Ok(ID);
            }
            else
            {
                return BadRequest("La solicitud es incorrecta. El ID no existe.");
            }
            
        }

        //Manejo de Edit (por ID)
        [HttpPut("Actualizar/{ID}")]
        public IActionResult Edit(int ID, [FromBody] Cliente value)
        {
            Memoria datos = new Memoria(Cache);
            var ClienteExistente = datos.GetListaClientes().Find(p => p.ID == ID);
            if (datos.ClienteExiste(ID))
            {
                if (datos.ClienteExiste(value.ID) && ClienteExistente.ID != value.ID)
                {
                    return BadRequest("La solicitud es incorrecta. El ID ingresado pertenece a otro usuario.");
                }
                else
                {
                    datos.ActualizarCliente(ID, value);
                    return Ok(value);
                }
            }
            else
            {
                return BadRequest("La solicitud es incorrecta. El ID no existe.");
            }
        }*/
    }
}
