using AndresBalladares_Proyecto.Models;
using AndresBalladares_Proyecto.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace AndresBalladares_Proyecto.Controllers
{
    public class ClientesController : Controller
    {
        //private readonly IMemoryCache _Cache;
        private readonly IServicioClientes _servicioClientes;

        public ClientesController(IServicioClientes servicioClientes)
        {
            //_Cache = Cache;
            _servicioClientes = servicioClientes;
        }

        private string GenerarClave(int id, string nombre, string apellidos)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellidos))
            {
                throw new ArgumentException("El nombre y los apellidos no pueden estar vacíos.");
            }

            string primerasDosLetrasNombre = nombre.Length >= 2 ? nombre.Substring(0, 2) : nombre;
            string primeraLetraApellido = char.ToUpper(apellidos[0]).ToString();

            return $"{id}{primerasDosLetrasNombre}{primeraLetraApellido}";
        }

        [HttpPost]
        public async Task<IActionResult> Store(Cliente model)
        {
            if (ModelState.IsValid)
            {
                model.Clave = GenerarClave(model.ID, model.Nombre, model.Apellidos);
                var lista = await _servicioClientes.Get();
                if (lista != null)
                {
                    var clienteExistente = lista.FirstOrDefault(i => i.ID == model.ID);
                    if (clienteExistente != null)
                    {
                        TempData["Errores"] = "Ya existe un cliente con el ID " + model.ID + ". Por favor, elija un ID diferente.";
                        return RedirectToAction("Create");
                    }
                }

                bool resultado = await _servicioClientes.Guardar(model);
                if (resultado)
                {
                    TempData["Mensaje"] = $"Los datos de {model.Nombre} {model.Apellidos} han sido guardados.<br>Usuario: {model.ID}<br>Contraseña: {model.Clave}";
                }
                else
                {
                    TempData["Errores"] = "Error al guardar el cliente.";
                }
                return RedirectToAction("Create");
            }
            else
            {
                TempData["Errores"] = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return RedirectToAction("Create");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Borrar(int ID)
        {
            bool resultado = await _servicioClientes.Eliminar(ID);

            if (resultado)
            {
                TempData["Mensaje"] = "El cliente ha sido eliminado.";
            }
            else
            {
                TempData["Errores"] = "Error al eliminar el cliente.";
            }
            return RedirectToAction("Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int ClienteSeleccionado, Cliente model)
        {
            var listaClientes = await _servicioClientes.Get();
            var clienteAEditar = listaClientes.Find(p => p.ID == ClienteSeleccionado);

            if (clienteAEditar != null)
            {
                var clienteExistente = listaClientes.FirstOrDefault(i => i.ID == model.ID);
                if (clienteExistente != null && model.ID != clienteAEditar.ID)
                {
                    TempData["Errores"] = "Ya existe un cliente con el ID " + model.ID + ". Por favor, elija un ID diferente.";
                    return RedirectToAction("Edit", listaClientes);
                }

                model.Clave = GenerarClave(model.ID, model.Nombre, model.Apellidos);
                bool resultado = await _servicioClientes.Actualizar(ClienteSeleccionado, model);
                if (resultado)
                {
                    TempData["Mensaje"] = $"Los datos de {model.Nombre} {model.Apellidos} han sido actualizados.<br>Usuario: {model.ID}<br>Contraseña: {model.Clave}";
                }
                else
                {
                    TempData["Errores"] = "Error al actualizar el cliente.";
                }
            }
            return RedirectToAction("Edit");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List <Cliente> clientes = await _servicioClientes.Get();
            return View(clientes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details()
        {
            List<Cliente> clientes = await _servicioClientes.Get();
            return View("Details", clientes);
        }

        [HttpGet]
        public async Task<IActionResult> Delete()
        {
            List<Cliente> clientes = await _servicioClientes.Get();
            return View("Delete", clientes);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            List<Cliente> clientes = await _servicioClientes.Get();
            return View("Edit", clientes);
        }

        /* Código realizado en el proyecto 1:
        public void CacheCliente(Cliente cliente)
        {
            var Clientes = _Cache.Get("Clientes") as List<Cliente>;
            if (Clientes == null)
            {
                Clientes = new List<Cliente>();
            }
            Clientes.Add(cliente);
            _Cache.Set("Clientes", Clientes, new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(15)
            });
        }

        private string GenerarClave(int id, string nombre, string apellidos)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellidos))
            {
                throw new ArgumentException("El nombre y los apellidos no pueden estar vacíos.");
            }

            string primerasDosLetrasNombre = nombre.Length >= 2 ? nombre.Substring(0, 2) : nombre;
            string primeraLetraApellido = char.ToUpper(apellidos[0]).ToString();

            return $"{id}{primerasDosLetrasNombre}{primeraLetraApellido}";
        }

        public List<Cliente> GetCacheCliente()
        {
            var Clientes = _Cache.Get("Clientes") as List<Cliente>;
            return Clientes;
        }

        [HttpPost]
        public IActionResult Store(Cliente model)
        {
            if (ModelState.IsValid)
            {
                model.Clave = GenerarClave(model.ID, model.Nombre, model.Apellidos);
                var Lista = GetCacheCliente();
                if (Lista != null)
                {
                    var clienteExistente = Lista.FirstOrDefault(i => i.ID == model.ID);
                    if (clienteExistente != null)
                    {
                        TempData["Errores"] = "Ya existe un cliente con el ID " + model.ID + ". Por favor, elija un ID diferente.";
                        return RedirectToAction("Create");
                    }
                }

                CacheCliente(model);
                var Cliente = GetCacheCliente();

                foreach (var Info in Cliente)
                {
                    TempData["Mensaje"] = $"Los datos de {Info.Nombre} {Info.Apellidos} han sido guardados.<br>Usuario: {Info.ID}<br>Contraseña: {Info.Clave}";
                }
                return RedirectToAction("Create");
            }
            else
            {
                TempData["Errores"] = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                return RedirectToAction("Create");
            }
        }

        [HttpPost]
        public IActionResult Borrar(int ID)
        {
            var ClienteAEliminar = GetCacheCliente().Find(p => p.ID == ID);

            if (ClienteAEliminar != null)
            {
                GetCacheCliente().Remove(ClienteAEliminar);
            }

            var ListaClientes = GetCacheCliente();
            return RedirectToAction("Delete", ListaClientes);
        }

        [HttpPost]
        public IActionResult Editar(int ClienteSeleccionado, Cliente model)
        {
            var ListaClientes = GetCacheCliente();
            var ClienteAEditar = GetCacheCliente().Find(p => p.ID == ClienteSeleccionado);
            
            if (ClienteAEditar != null)
            {
                var clienteExistente = GetCacheCliente().FirstOrDefault(i => i.ID == model.ID);
                if (clienteExistente != null && model.ID != ClienteAEditar.ID)
                {
                    TempData["Errores"] = "Ya existe un cliente con el ID " + model.ID + ". Por favor, elija un ID diferente.";
                    return RedirectToAction("Edit", ListaClientes);
                }

                GetCacheCliente().Remove(ClienteAEditar);
                ClienteAEditar = model;
                ClienteAEditar.Clave = GenerarClave(ClienteAEditar.ID, ClienteAEditar.Nombre, ClienteAEditar.Apellidos);
                GetCacheCliente().Add(ClienteAEditar);
            }
            
            foreach (var Info in ListaClientes)
            {
                TempData["Mensaje"] = $"Los datos de {Info.Nombre} {Info.Apellidos} han sido guardados.<br>Usuario: {Info.ID}<br>Contraseña: {Info.Clave}";
            }
            return RedirectToAction("Edit", ListaClientes);
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Details()
        {
            var clientes = GetCacheCliente();

            return View("Details", clientes);
        }
        public IActionResult Delete()
        {
            var clientes = GetCacheCliente();

            return View("Delete", clientes);
        }
        public IActionResult Edit()
        {
            var clientes = GetCacheCliente();

            return View("Edit", clientes);
        }*/
    }
}
