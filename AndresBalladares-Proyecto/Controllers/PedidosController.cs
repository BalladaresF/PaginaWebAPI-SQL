using AndresBalladares_Proyecto.Models;
using AndresBalladares_Proyecto.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace AndresBalladares_Proyecto.Controllers
{
    public class PedidosController : Controller
    {
        //private readonly IMemoryCache _Cache;
        //private readonly InventarioController _inventarioController;
        //private readonly ClientesController _clientesController;
        private readonly IServicioPedidos _servicioPedidos;
        private readonly IServicioInventario _servicioInventario;
        private readonly IServicioClientes _servicioClientes;
        private readonly IServicioDirecciones _servicioDirecciones;

        public PedidosController(IServicioPedidos servicioPedidos, IServicioInventario servicioInventario, IServicioClientes servicioClientes, IServicioDirecciones servicioDirecciones)
        {
            //_Cache = Cache;
            //_inventarioController = inventarioController;
            //_clientesController = clientesController;
            _servicioPedidos = servicioPedidos;
            _servicioInventario = servicioInventario;
            _servicioClientes = servicioClientes;
            _servicioDirecciones = servicioDirecciones;
        }

        [HttpPost]
        public async Task<IActionResult> Store(Pedido model)
        {
            if (ModelState.IsValid)
            {
                var lista = await _servicioPedidos.Get();
                if (lista != null)
                {
                    var pedidoExistente = lista.FirstOrDefault(i => i.ID == model.ID);
                    if (pedidoExistente != null)
                    {
                        TempData["Errores"] = "Ya existe un pedido con el ID " + model.ID + ". Por favor, elija un ID diferente.";
                        TempData["IDCliente"] = model.IDCliente;
                        return RedirectToAction("Create");
                    }
                }

                // Detección del artículo del inventario y el cliente:
                var inventario = await _servicioInventario.Get();
                Inventario ArticuloSeleccionado = inventario.FirstOrDefault(i => i.ID == model.IDInventario);
                var cliente = await _servicioClientes.Get();
                Cliente ClienteSeleccionado = cliente.FirstOrDefault(i => i.ID == model.IDCliente);
                double montoDescuento = 0;

                // Cálculo de los precios:
                if (ArticuloSeleccionado != null)
                {
                    model.CostoSinIva = ArticuloSeleccionado.Precio * model.Cantidad;

                    double Descuento = ClienteSeleccionado.DineroCompradoUltimosSeisMeses switch
                    {
                        >= 500000 => 0.06,
                        >= 400000 => 0.05,
                        >= 300000 => 0.04,
                        >= 200000 => 0.03,
                        _ => 0
                    };

                    model.Costo = (int)(model.CostoSinIva + (model.CostoSinIva * 0.13));

                    montoDescuento = model.Costo * Descuento;
                    double costoConDescuento = model.Costo - montoDescuento;

                    model.Costo = (int)(costoConDescuento);
                }

                bool resultado = await _servicioPedidos.Guardar(model);
                if (resultado)
                {
                    TempData["Mensaje"] = "Los datos del pedido " + model.ID + " han sido guardados. Precio final: " + model.Costo + " colones. Descuento realizado: " + montoDescuento + " colones.";
                }
                else
                {
                    TempData["Errores"] = "Error al guardar el pedido.";
                }
                TempData["IDCliente"] = model.IDCliente;
                return RedirectToAction("Create");
            }
            else
            {
                TempData["Errores"] = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["IDCliente"] = model.IDCliente;
                return RedirectToAction("Create");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Borrar(int ID)
        {
            bool resultado = await _servicioPedidos.Eliminar(ID);

            if (resultado)
            {
                TempData["Mensaje"] = "El pedido ha sido eliminado.";
            }
            else
            {
                TempData["Errores"] = "Error al eliminar el pedido.";
            }
            return RedirectToAction("Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int PedidoSeleccionado, Pedido model)
        {
            var listaPedidos = await _servicioPedidos.Get();
            var pedidoAEditar = listaPedidos.Find(p => p.ID == PedidoSeleccionado);

            if (pedidoAEditar != null)
            {
                var pedidoExistente = listaPedidos.FirstOrDefault(i => i.ID == model.ID);
                if (pedidoExistente != null && model.ID != pedidoAEditar.ID)
                {
                    TempData["Errores"] = "Ya existe un pedido con el ID " + model.ID + ". Por favor, elija un ID diferente.";
                    return RedirectToAction("Edit");
                }
                model.IDCliente = pedidoAEditar.IDCliente;

                // Detección del artículo del inventario y el cliente:
                var inventario = await _servicioInventario.Get();
                Inventario ArticuloSeleccionado = inventario.FirstOrDefault(i => i.ID == model.IDInventario);
                var cliente = await _servicioClientes.Get();
                Cliente ClienteSeleccionado = cliente.FirstOrDefault(i => i.ID == model.IDCliente);
                double montoDescuento = 0;

                // Cálculo de los precios:
                if (ArticuloSeleccionado != null)
                {
                    model.CostoSinIva = ArticuloSeleccionado.Precio * model.Cantidad;

                    double Descuento = ClienteSeleccionado.DineroCompradoUltimosSeisMeses switch
                    {
                        >= 500000 => 0.06,
                        >= 400000 => 0.05,
                        >= 300000 => 0.04,
                        >= 200000 => 0.03,
                        _ => 0
                    };

                    model.Costo = (int)(model.CostoSinIva + (model.CostoSinIva * 0.13));

                    montoDescuento = model.Costo * Descuento;
                    double costoConDescuento = model.Costo - montoDescuento;

                    model.Costo = (int)(costoConDescuento);
                }

                bool resultado = await _servicioPedidos.Actualizar(PedidoSeleccionado, model);
                if (resultado)
                {
                    TempData["Mensaje"] = "Los datos del pedido " + model.ID + " han sido actualizados. Precio final: " + model.Costo + " colones. Descuento realizado: " + montoDescuento + " colones.";
                }
                else
                {
                    TempData["Errores"] = "Error al actualizar el pedido.";
                }
            }
            return RedirectToAction("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> VerificarUsuario(Login model)
        {
            var clientes = await _servicioClientes.Get();
            Cliente ClienteSeleccionado = clientes.FirstOrDefault(item => item.ID == model.ID && item.Clave == model.clave);

            if (ClienteSeleccionado != null)
            {
                var viewModel = new ModelosViewModel
                {
                    Inventarios = await _servicioInventario.Get(),
                    Direcciones = await _servicioDirecciones.Get()
                };
                TempData["IDCliente"] = ClienteSeleccionado.ID;
                return View("Create", viewModel);
            }
            else
            {
                TempData["Errores"] = "El usuario y contraseña ingresados no pertenece a ningún cliente.";
                return RedirectToAction("InicioSesion");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var pedidos = await _servicioPedidos.Get();
            return View(pedidos);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new ModelosViewModel
            {
                Inventarios = await _servicioInventario.Get(),
                Direcciones = await _servicioDirecciones.Get()
            };

            return View("Create", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var pedidos = await _servicioPedidos.Get();
            return View("Details", pedidos);
        }

        [HttpGet]
        public async Task<IActionResult> Delete()
        {
            var pedidos = await _servicioPedidos.Get();
            return View("Delete", pedidos);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var viewModel = new ModelosViewModel
            {
                Inventarios = await _servicioInventario.Get(),
                Pedidos = await _servicioPedidos.Get(),
                Direcciones = await _servicioDirecciones.Get()
            };

            return View("Edit", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> InicioSesion()
        {
            var clientes = await _servicioClientes.Get();
            return View("InicioSesion", clientes);
        }

        /* Código realizado utilizando cache:
        public void CachePedido(Pedido pedido)
        {
            var Pedidos = _Cache.Get("Pedidos") as List<Pedido>;
            if (Pedidos == null)
            {
                Pedidos = new List<Pedido>();
            }
            Pedidos.Add(pedido);
            _Cache.Set("Pedidos", Pedidos, new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(15)
            });
        }

        public List<Pedido> GetCachePedido()
        {
            var Pedidos = _Cache.Get("Pedidos") as List<Pedido>;
            return Pedidos;
        }

        public List<Inventario> GetCacheInventario()
        {
            var Inventario = _inventarioController.GetCacheInventario();
            return Inventario;
        }

        public List<Cliente> GetCacheCliente()
        {
            var Clientes = _Cache.Get("Clientes") as List<Cliente>;
            return Clientes;
        }

        public List<Direccion> GetCacheDireccion()
        {
            var Direcciones = _Cache.Get("Direcciones") as List<Direccion>;
            return Direcciones;
        }

        [HttpPost]
        public IActionResult Store(Pedido model)
        {
            if (ModelState.IsValid)
            {
                var Lista = GetCachePedido();
                if (Lista!=null)
                {
                    var pedidoExistente = GetCachePedido().FirstOrDefault(i => i.ID == model.ID);
                    if (pedidoExistente != null)
                    {
                        TempData["Errores"] = "Ya existe un pedido con el ID " + model.ID + ". Por favor, elija un ID diferente.";
                        TempData["IDCliente"] = model.IDCliente;
                        return RedirectToAction("Create");
                    }
                }
                
                //Detección del artículo del inventario:
                var inventario = GetCacheInventario();
                Inventario ArticuloSeleccionado = null;
                foreach (var item in inventario)
                {
                    if (model.IDInventario == item.ID)
                    {
                        ArticuloSeleccionado = item;
                    }
                }

                //Cálculo de los precios:
                if (ArticuloSeleccionado != null)
                {
                    //Cálculo de los precios:
                    model.CostoSinIva = ArticuloSeleccionado.Precio * model.Cantidad;
                    model.Costo = (int)(model.CostoSinIva + (model.CostoSinIva * 0.13));
                }

                CachePedido(model);
                var Pedido = GetCachePedido();

                foreach (var Info in Pedido)
                {
                    TempData["Mensaje"] = "Los datos del pedido " + Info.ID + " han sido guardados. Precio final: "+ Info.Costo + " colones.";
                }
                TempData["IDCliente"] = model.IDCliente;
                return RedirectToAction("Create");
            }
            else
            {
                TempData["Errores"] = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["IDCliente"] = model.IDCliente;
                return RedirectToAction("Create");
            }
        }

        [HttpPost]
        public IActionResult Borrar(int ID)
        {
            var PedidoAEliminar = GetCachePedido().Find(p => p.ID == ID);

            if (PedidoAEliminar != null)
            {
                GetCachePedido().Remove(PedidoAEliminar);
            }

            var ListaPedidos = GetCachePedido();
            return RedirectToAction("Delete", ListaPedidos);
        }

        [HttpPost]
        public IActionResult Editar(int PedidoSeleccionado, Pedido model)
        {
            var ListaPedidos = GetCachePedido();
            var PedidoAEditar = GetCachePedido().Find(p => p.ID == PedidoSeleccionado);

            if (PedidoAEditar != null)
            {
                var pedidoExistente = GetCachePedido().FirstOrDefault(i => i.ID == model.ID);
                if (pedidoExistente != null && model.ID != PedidoAEditar.ID)
                {
                    TempData["Errores"] = "Ya existe un pedido con el ID " + model.ID + ". Por favor, elija un ID diferente.";
                    return RedirectToAction("Edit", ListaPedidos);
                }

                //Detección del artículo del inventario:
                var inventario = GetCacheInventario();
                Inventario ArticuloSeleccionado = null;
                foreach (var item in inventario)
                {
                    if (model.IDInventario == item.ID)
                    {
                        ArticuloSeleccionado = item;
                    }
                }
                if (ArticuloSeleccionado!=null)
                {
                    //Cálculo de los precios:
                    model.CostoSinIva = ArticuloSeleccionado.Precio * model.Cantidad;
                    model.Costo = (int)(model.CostoSinIva + (model.CostoSinIva * 0.13));
                }
                int IDCliente = PedidoAEditar.IDCliente;
                GetCachePedido().Remove(PedidoAEditar);
                PedidoAEditar = model;
                PedidoAEditar.IDCliente = IDCliente;
                GetCachePedido().Add(PedidoAEditar);
            }
            
            foreach (var Info in ListaPedidos)
            {
                TempData["Mensaje"] = "Los datos de " + Info.ID + " - " + Info.Costo + " - " + Info.Estado + " han sido actualizados.";
            }
            return RedirectToAction("Edit", ListaPedidos);
        }

        [HttpPost]
        public IActionResult VerificarUsuario(Login model)
        {
            var clientes = GetCacheCliente();
            Cliente ClienteSeleccionado = null;
            foreach (var item in clientes)
            {
                if (model.ID == item.ID && model.clave == item.Clave)
                {
                    ClienteSeleccionado = item;
                }
            }
            if (ClienteSeleccionado != null)
            {
                var viewModel = new ModelosViewModel
                {
                    Inventarios = GetCacheInventario(),
                    Direcciones = GetCacheDireccion()
                };
                TempData["IDCliente"] = ClienteSeleccionado.ID;
                return View("Create", viewModel);
            }
            else
            {
                TempData["Errores"] = "El usuario y contraseña ingresados no pertenece a ningún cliente.";
                return RedirectToAction("InicioSesion", clientes);
            }
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            var viewModel = new ModelosViewModel
            {
                Inventarios = GetCacheInventario(),
                Direcciones = GetCacheDireccion()
            };

            return View("Create", viewModel);
        }
        public IActionResult Details()
        {
            var pedidos = GetCachePedido();

            return View("Details", pedidos);
        }
        public IActionResult Delete()
        {
            var pedidos = GetCachePedido();

            return View("Delete", pedidos);
        }
        public IActionResult Edit()
        {
            var viewModel = new ModelosViewModel
            {
                Inventarios = GetCacheInventario(),
                Pedidos = GetCachePedido(),
                Direcciones = GetCacheDireccion()
            };

            return View("Edit", viewModel);
        }

        public IActionResult InicioSesion()
        {
            var Clientes = GetCacheCliente();
            return View("InicioSesion", Clientes);
        }*/
    }
}
