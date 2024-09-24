using AndresBalladares_Proyecto.Models;
using AndresBalladares_Proyecto.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualBasic;
using System.Collections;
using System.Collections.Generic;

namespace AndresBalladares_Proyecto.Controllers
{
    public class InventarioController : Controller
    {
        //private readonly IMemoryCache _Cache;
        private readonly IServicioInventario _servicioInventario;

        public InventarioController(IServicioInventario servicioInventario)
        {
            //_Cache = Cache;
            _servicioInventario = servicioInventario;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var inventario = await _servicioInventario.Get();
            return View(inventario);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Store(Inventario model)
        {
            if (ModelState.IsValid)
            {
                var lista = await _servicioInventario.Get();
                if (lista != null)
                {
                    var inventarioExistente = lista.FirstOrDefault(i => i.ID == model.ID);
                    if (inventarioExistente != null)
                    {
                        TempData["Errores"] = "Ya existe un inventario con el ID " + model.ID + ". Por favor, elija un ID diferente.";
                        return RedirectToAction("Create");
                    }
                }

                if (model.Precio <= 0)
                {
                    TempData["Errores"] = "El precio establecido debe ser mayor a 0";
                    return RedirectToAction("Create");
                }

                bool resultado = await _servicioInventario.Guardar(model);
                if (resultado)
                {
                    TempData["Mensaje"] = "Los datos del artículo \"" + model.Tipo + "\" han sido guardados.";
                }
                else
                {
                    TempData["Errores"] = "Error al guardar el inventario.";
                }
                return RedirectToAction("Create");
            }
            else
            {
                TempData["Errores"] = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return RedirectToAction("Create");
            }
        }

        [HttpGet]
        public IActionResult Details()
        {
            var inventario = _servicioInventario.Get().Result;
            return View("Details", inventario);
        }

        [HttpGet]
        public async Task<IActionResult> Delete()
        {
            var inventario = await _servicioInventario.Get();
            return View(inventario);
        }

        [HttpPost]
        public async Task<IActionResult> Borrar(int ID)
        {
            bool resultado = await _servicioInventario.Eliminar(ID);

            if (resultado)
            {
                TempData["Mensaje"] = "El inventario ha sido eliminado.";
            }
            else
            {
                TempData["Errores"] = "Error al eliminar el inventario.";
            }
            return RedirectToAction("Delete");
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var inventario = await _servicioInventario.Get();
            return View(inventario);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int InventarioSeleccionado, Inventario model)
        {
            var listaInventario = await _servicioInventario.Get();
            var inventarioAEditar = listaInventario.Find(p => p.ID == InventarioSeleccionado);

            if (inventarioAEditar != null)
            {
                var inventarioExistente = listaInventario.FirstOrDefault(i => i.ID == model.ID);
                if (inventarioExistente != null && model.ID != inventarioAEditar.ID)
                {
                    TempData["Errores"] = "Ya existe un inventario con el ID " + model.ID + ". Por favor, elija un ID diferente.";
                    return RedirectToAction("Edit", listaInventario);
                }

                if (model.Precio <= 0)
                {
                    TempData["Errores"] = "El precio establecido debe ser mayor a 0";
                    return RedirectToAction("Edit", listaInventario);
                }

                bool resultado = await _servicioInventario.Actualizar(InventarioSeleccionado, model);
                if (resultado)
                {
                    TempData["Mensaje"] = "Los datos de " + model.ID + " - " + model.Tipo + " han sido actualizados.";
                }
                else
                {
                    TempData["Errores"] = "Error al actualizar el inventario.";
                }
            }
            return RedirectToAction("Edit");
        }

        /* Código realizado en el proyecto 1:
        public void CacheInventario(Inventario inventario)
        {
            var Inventarios = _Cache.Get("Inventario") as List<Inventario>;
            if (Inventarios == null)
            {
                Inventarios = new List<Inventario>();
            }
            Inventarios.Add(inventario);
            _Cache.Set("Inventario", Inventarios, new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(15)
            });
        }

        public List<Inventario> GetCacheInventario()
        {
            var Inventarios = _Cache.Get("Inventario") as List<Inventario>;
            return Inventarios;
        }

        [HttpPost]
        public IActionResult Store(Inventario model)
        {
            if (ModelState.IsValid)
            {
                var Lista = GetCacheInventario();
                if (Lista != null)
                {
                    var inventarioExistente = Lista.FirstOrDefault(i => i.ID == model.ID);
                    if (inventarioExistente != null)
                    {
                        TempData["Errores"] = "Ya existe un inventario con el ID " + model.ID + ". Por favor, elija un ID diferente.";
                        return RedirectToAction("Create");
                    }
                }

                if (model.Precio <= 0)
                {
                    TempData["Errores"] = "El precio establecido debe ser mayor a 0";
                    return RedirectToAction("Create");
                }

                CacheInventario(model);
                var Inventario = GetCacheInventario();

                foreach (var Info in Inventario)
                {
                    TempData["Mensaje"] = "Los datos del artículo \"" + Info.Tipo + "\" han sido guardados.";
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
            var InventarioAEliminar = GetCacheInventario().Find(p => p.ID == ID);

            if (InventarioAEliminar != null)
            {
                GetCacheInventario().Remove(InventarioAEliminar);
            }

            var ListaInventario = GetCacheInventario();
            return RedirectToAction("Delete", ListaInventario);
        }

        [HttpPost]
        public IActionResult Editar(int InventarioSeleccionado, Inventario model)
        {
            var ListaInventario = GetCacheInventario();
            var InventarioAEditar = GetCacheInventario().Find(p => p.ID == InventarioSeleccionado);

            if (InventarioAEditar != null)
            {
                var inventarioExistente = GetCacheInventario().FirstOrDefault(i => i.ID == model.ID);
                if (inventarioExistente != null && model.ID != InventarioAEditar.ID)
                {
                    TempData["Errores"] = "Ya existe un inventario con el ID " + model.ID + ". Por favor, elija un ID diferente.";
                    return RedirectToAction("Edit", ListaInventario);
                }

                if (model.Precio <= 0)
                {
                    TempData["Errores"] = "El precio establecido debe ser mayor a 0";
                    return RedirectToAction("Edit", ListaInventario);
                }

                GetCacheInventario().Remove(InventarioAEditar);
                InventarioAEditar = model;
                GetCacheInventario().Add(InventarioAEditar);
            }
            
            foreach (var Info in ListaInventario)
            {
                TempData["Mensaje"] = "Los datos de " + Info.ID + " - "+ Info.Tipo + " han sido actualizados.";
            }
            return RedirectToAction("Edit", ListaInventario);
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
            var inventario = GetCacheInventario();

            return View("Details", inventario);
        }
        public IActionResult Delete()
        {
            var inventario = GetCacheInventario();

            return View("Delete", inventario);
        }
        public IActionResult Edit()
        {
            var inventario = GetCacheInventario();

            return View("Edit", inventario);
        }*/
    }
}
