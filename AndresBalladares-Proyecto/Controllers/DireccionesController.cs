using AndresBalladares_Proyecto.Models;
using AndresBalladares_Proyecto.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace AndresBalladares_Proyecto.Controllers
{
    public class DireccionesController : Controller
    {
        //private readonly IMemoryCache _Cache;
        private readonly IServicioDirecciones _servicioDirecciones;
        private readonly IServicioClientes _servicioClientes;

        public DireccionesController(IServicioDirecciones servicioDirecciones, IServicioClientes servicioClientes)
        {
            //_Cache = Cache;
            _servicioDirecciones = servicioDirecciones;
            _servicioClientes = servicioClientes;
        }

        [HttpPost]
        public async Task<IActionResult> Store(Direccion model)
        {
            if (ModelState.IsValid)
            {
                var lista = await _servicioDirecciones.Get();
                if (lista != null)
                {
                    var direccionExistente = lista.FirstOrDefault(i => i.ID == model.ID);
                    if (direccionExistente != null)
                    {
                        TempData["Errores"] = "Ya existe una dirección con el ID " + model.ID + ". Por favor, elija un ID diferente.";
                        return RedirectToAction("Create");
                    }
                }

                bool resultado = await _servicioDirecciones.Guardar(model);
                if (resultado)
                {
                    TempData["Mensaje"] = "Los datos de la dirección \"" + model.ID + "\" del cliente " + model.IDCliente + " han sido guardados.";
                }
                else
                {
                    TempData["Errores"] = "Error al guardar la dirección.";
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
            bool resultado = await _servicioDirecciones.Eliminar(ID);

            if (resultado)
            {
                TempData["Mensaje"] = "La dirección ha sido eliminada.";
            }
            else
            {
                TempData["Errores"] = "Error al eliminar la dirección.";
            }
            return RedirectToAction("Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int DireccionSeleccionada, Direccion model)
        {
            var listaDirecciones = await _servicioDirecciones.Get();
            var direccionAEditar = listaDirecciones.Find(p => p.ID == DireccionSeleccionada);

            if (direccionAEditar != null)
            {
                var direccionExistente = listaDirecciones.FirstOrDefault(i => i.ID == model.ID);
                if (direccionExistente != null && model.ID != direccionAEditar.ID)
                {
                    TempData["Errores"] = "Ya existe una dirección con el ID " + model.ID + ". Por favor, elija un ID diferente.";
                    return RedirectToAction("Edit", listaDirecciones);
                }

                bool resultado = await _servicioDirecciones.Actualizar(DireccionSeleccionada, model);
                if (resultado)
                {
                    TempData["Mensaje"] = "Los datos de la dirección \"" + model.ID + "\" del cliente " + model.IDCliente + " han sido actualizados.";
                }
                else
                {
                    TempData["Errores"] = "Error al actualizar la dirección.";
                }
            }
            return RedirectToAction("Edit");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var direcciones = await _servicioDirecciones.Get();
            return View(direcciones);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var clientes = await _servicioClientes.Get();
            return View("Create", clientes);
        }

        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var direcciones = await _servicioDirecciones.Get();
            return View("Details", direcciones);
        }

        [HttpGet]
        public async Task<IActionResult> Delete()
        {
            var direcciones = await _servicioDirecciones.Get();
            return View("Delete", direcciones);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var viewModel = new ModelosViewModel
            {
                Clientes = await _servicioClientes.Get(),
                Direcciones = await _servicioDirecciones.Get()
            };

            return View("Edit", viewModel);
        }

        /* Código realizado en el proyecto 1:
        public void CacheDireccion(Direccion direccion)
        {
            var Direcciones = _Cache.Get("Direcciones") as List<Direccion>;
            if (Direcciones == null)
            {
                Direcciones = new List<Direccion>();
            }
            Direcciones.Add(direccion);
            _Cache.Set("Direcciones", Direcciones, new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(15)
            });
        }

        public List<Direccion> GetCacheDireccion()
        {
            var Direcciones = _Cache.Get("Direcciones") as List<Direccion>;
            return Direcciones;
        }

        public List<Cliente> GetCacheCliente()
        {
            var Clientes = _Cache.Get("Clientes") as List<Cliente>;
            return Clientes;
        }

        [HttpPost]
        public IActionResult Store(Direccion model)
        {
            if (ModelState.IsValid)
            {
                var Lista = GetCacheDireccion();
                if (Lista != null)
                {
                    var DireccionExistente = Lista.FirstOrDefault(i => i.ID == model.ID);
                    if (DireccionExistente != null)
                    {
                        TempData["Errores"] = "Ya existe una dirección con el ID " + model.ID + ". Por favor, elija un ID diferente.";
                        return RedirectToAction("Create");
                    }
                }

                CacheDireccion(model);
                var Direccion = GetCacheDireccion();

                foreach (var Info in Direccion)
                {
                    TempData["Mensaje"] = "Los datos de la dirección \"" + Info.ID + "\" del cliente " + Info.IDCliente + " han sido guardados.";
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
            var DireccionAEliminar = GetCacheDireccion().Find(p => p.ID == ID);

            if (DireccionAEliminar != null)
            {
                GetCacheDireccion().Remove(DireccionAEliminar);
            }

            var ListaDirecciones = GetCacheDireccion();
            return RedirectToAction("Delete", ListaDirecciones);
        }

        [HttpPost]
        public IActionResult Editar(int DireccionSeleccionada, Direccion model)
        {
            var ListaDirecciones = GetCacheDireccion();
            var DireccionAEditar = GetCacheDireccion().Find(p => p.ID == DireccionSeleccionada);

            if (DireccionAEditar != null)
            {
                var direccionExistente = GetCacheDireccion().FirstOrDefault(i => i.ID == model.ID);
                if (direccionExistente != null && model.ID != DireccionAEditar.ID)
                {
                    TempData["Errores"] = "Ya existe una dirección con el ID " + model.ID + ". Por favor, elija un ID diferente.";
                    return RedirectToAction("Edit", ListaDirecciones);
                }

                GetCacheDireccion().Remove(DireccionAEditar);
                DireccionAEditar = model;
                GetCacheDireccion().Add(DireccionAEditar);
            }

            foreach (var Info in ListaDirecciones)
            {
                TempData["Mensaje"] = "Los datos de la dirección \"" + Info.ID + "\" del cliente " + Info.IDCliente + " han sido actualizados.";
            }
            return RedirectToAction("Edit", ListaDirecciones);
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            var clientes = GetCacheCliente();
            return View("Create", clientes);
        }
        public IActionResult Details()
        {
            var direcciones = GetCacheDireccion();

            return View("Details", direcciones);
        }
        public IActionResult Delete()
        {

            var direcciones = GetCacheDireccion();

            return View("Delete", direcciones);
        }
        public IActionResult Edit()
        {
            var viewModel = new ModelosViewModel
            {
                Clientes = GetCacheCliente(),
                Direcciones = GetCacheDireccion()
            };

            return View("Edit", viewModel);
        }*/
    }
}
