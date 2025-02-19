using AndresBalladares_Proyecto.API.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using Microsoft.EntityFrameworkCore; //se instaló la versión 7.0.20 para que sea compatible con .NET 7.
using AndresBalladares_Proyecto.Models;

/*
 * En esta clase se realiza todo el manejo de información. Esta clase es utilizada por los controladores 
 * del API, los cuales son luego utilizados para mover los datos a la UI y viceversa.
 */

namespace AndresBalladares_Proyecto.API.Data
{
    public class Memory
    {
        //private readonly IMemoryCache Cache;
        private readonly Context DbContext;
        public Memory(Context dbContext)
        {
            //Cache = cache;
            DbContext = dbContext;
        }

        //Verificaciones de si el cache está vacío:
        public bool ClientesVacio()
        {
            return !DbContext.Clientes.Any();
        }

        public bool InventarioVacio()
        {
            return !DbContext.Inventarios.Any();
        }
        public bool PedidosVacio()
        {
            return !DbContext.Pedidos.Any();
        }
        public bool DireccionesVacio()
        {
            return !DbContext.Direcciones.Any();
        }

        //Obtener listas:
        public List<Models.Cliente> GetListaClientes()
        {
            var Resultado = from c in DbContext.Clientes
                            select c;
            return Resultado.ToList();
        }

        public List<Models.Inventario> GetListaInventario()
        {
            var Resultado = from c in DbContext.Inventarios
                            select c;
            return Resultado.ToList();
        }

        public List<Models.Pedido> GetListaPedidos()
        {
            var Resultado = from c in DbContext.Pedidos
                            select c;
            return Resultado.ToList();
        }

        public List<Models.Direccion> GetListaDirecciones()
        {
            var Resultado = from c in DbContext.Direcciones
                            select c;
            return Resultado.ToList();
        }

        //Verificar si existen elementos de los modelos:
        public bool InventarioExiste(int ID)
        {
            return DbContext.Inventarios.Any(c => c.ID == ID);
        }

        public bool ClienteExiste(int ID)
        {
            return DbContext.Clientes.Any(c => c.ID == ID);
        }

        public bool PedidoExiste(int ID)
        {
            return DbContext.Pedidos.Any(c => c.ID == ID);
        }
        public bool DireccionExiste(int ID)
        {
            return DbContext.Direcciones.Any(c => c.ID == ID);
        }

        //Actualizar montos del cliente cada que se realiza un pedido:
        public void ActualizarMontoCliente(int IDCliente, Models.Pedido pedido)
        {
            var cliente = DbContext.Clientes.Find(IDCliente);
            if (cliente != null)
            {
                cliente.DineroCompradoTotal += pedido.Costo;

                DateTime fechaLimiteUltimoAño = DateTime.Now.AddYears(-1);
                DateTime fechaLimiteUltimosSeisMeses = DateTime.Now.AddMonths(-6);

                if (pedido.Fecha >= fechaLimiteUltimoAño)
                {
                    cliente.DineroCompradoUltimoAño += pedido.Costo;
                }
                if (pedido.Fecha >= fechaLimiteUltimosSeisMeses)
                {
                    cliente.DineroCompradoUltimosSeisMeses += pedido.Costo;
                }

                DbContext.SaveChanges();
            }
        }

        //Eliminar montos de pedidos en los montos de los clientes cuando los pedidos se eliminan o actualizan:
        public void EliminarMontoCliente(int IDCliente, Models.Pedido pedido)
        {
            var cliente = DbContext.Clientes.Find(IDCliente);
            if (cliente != null)
            {
                cliente.DineroCompradoTotal -= pedido.Costo;

                DateTime fechaLimiteUltimoAño = DateTime.Now.AddYears(-1);
                DateTime fechaLimiteUltimosSeisMeses = DateTime.Now.AddMonths(-6);

                if (pedido.Fecha >= fechaLimiteUltimoAño)
                {
                    cliente.DineroCompradoUltimoAño -= pedido.Costo;
                }
                if (pedido.Fecha >= fechaLimiteUltimosSeisMeses)
                {
                    cliente.DineroCompradoUltimosSeisMeses -= pedido.Costo;
                }

                DbContext.SaveChanges();
            }
        }

        //Agregar elementos:
        public void AgregarCliente(Models.Cliente cliente)
        {
            DbContext.Clientes.Add(cliente);
            DbContext.SaveChanges();
        }

        public void AgregarInventario(Models.Inventario inventario)
        {
            DbContext.Inventarios.Add(inventario);
            DbContext.SaveChanges();
        }

        public void AgregarPedido(Models.Pedido pedido)
        {
            DbContext.Pedidos.Add(pedido);
            ActualizarMontoCliente(pedido.IDCliente, pedido);
            DbContext.SaveChanges();
        }

        public void AgregarDireccion(Models.Direccion direccion)
        {
            DbContext.Direcciones.Add(direccion);
            DbContext.SaveChanges();
        }

        //Eliminar elementos:
        public void EliminarCliente(int ID)
        {
            if (ClienteExiste(ID))
            {
                var cliente = DbContext.Clientes.Find(ID);
                DbContext.Clientes.Remove(cliente);
                DbContext.SaveChanges();
            }
        }
        public void EliminarInventario(int ID)
        {
            if (InventarioExiste(ID))
            {
                var inventario = DbContext.Inventarios.Find(ID);
                DbContext.Inventarios.Remove(inventario);
                DbContext.SaveChanges();
            }
        }

        public void EliminarPedido(int ID)
        {
            if (PedidoExiste(ID))
            {
                var pedido = DbContext.Pedidos.Find(ID);
                DbContext.Pedidos.Remove(pedido);
                EliminarMontoCliente(pedido.IDCliente, pedido);
                DbContext.SaveChanges();
            }
        }

        public void EliminarDireccion(int ID)
        {
            if (DireccionExiste(ID))
            {
                var direccion = DbContext.Direcciones.Find(ID);
                DbContext.Direcciones.Remove(direccion);
                DbContext.SaveChanges();
            }
        }

        //Actualizar elementos:
        /*public void ActualizarCliente(int ID, Models.Cliente value)    //Este ActuaizarCliente NO actualiza la ID.
        {
            var cliente = DbContext.Clientes.Find(ID);
            if (cliente != null)
            {
                cliente.Nombre = value.Nombre;
                cliente.Apellidos = value.Apellidos;
                cliente.DineroCompradoTotal = value.DineroCompradoTotal;
                cliente.DineroCompradoUltimoAño = value.DineroCompradoUltimoAño;
                cliente.DineroCompradoUltimosSeisMeses = value.DineroCompradoUltimosSeisMeses;
                cliente.Clave = value.Clave;
                DbContext.SaveChanges();
            }
        }*/

        public void ActualizarCliente(int ID, Models.Cliente value)
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    // Obtener el cliente original
                    var cliente = DbContext.Clientes.Find(ID);
                    if (cliente != null)
                    {
                        // Obtener todas las entidades dependientes. Esto debe realizarse en orden.
                        var pedidos = DbContext.Pedidos.Where(p => p.IDCliente == ID).ToList();
                        var direcciones = DbContext.Direcciones.Where(d => d.IDCliente == ID).ToList();
                        var reportes = DbContext.Reportes.Where(r => r.IDCliente == ID).ToList();

                        // Primero eliminar los pedidos
                        DbContext.Pedidos.RemoveRange(pedidos);
                        DbContext.SaveChanges();

                        // Luego eliminar las direcciones
                        DbContext.Direcciones.RemoveRange(direcciones);
                        DbContext.SaveChanges();

                        // Luego eliminar los registros en Reportes
                        DbContext.Reportes.RemoveRange(reportes);
                        DbContext.SaveChanges();

                        // Eliminar el cliente original
                        DbContext.Clientes.Remove(cliente);
                        DbContext.SaveChanges();

                        // Crear un nuevo cliente con los datos actualizados
                        var nuevoCliente = new Models.Cliente
                        {
                            ID = value.ID,
                            Nombre = value.Nombre,
                            Apellidos = value.Apellidos,
                            DineroCompradoTotal = value.DineroCompradoTotal,
                            DineroCompradoUltimoAño = value.DineroCompradoUltimoAño,
                            DineroCompradoUltimosSeisMeses = value.DineroCompradoUltimosSeisMeses,
                            Clave = value.Clave
                        };
                        DbContext.Clientes.Add(nuevoCliente);
                        DbContext.SaveChanges();

                        // Actualizar las entidades dependientes con el nuevo ID del cliente y volver a insertarlas
                        foreach (var direccion in direcciones)
                        {
                            direccion.IDCliente = nuevoCliente.ID;
                            DbContext.Direcciones.Add(direccion);
                        }
                        DbContext.SaveChanges();

                        foreach (var pedido in pedidos)
                        {
                            pedido.IDCliente = nuevoCliente.ID;
                            DbContext.Pedidos.Add(pedido);
                        }
                        DbContext.SaveChanges();

                        foreach (var reporte in reportes)
                        {
                            reporte.IDCliente = nuevoCliente.ID;
                            DbContext.Reportes.Add(reporte);
                        }
                        DbContext.SaveChanges();

                        transaction.Commit();
                    }
                    else
                    {
                        throw new Exception("Cliente no encontrado.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    var innerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    throw new Exception($"{innerExceptionMessage}", ex);
                }
            }
        }

        /*public void ActualizarInventario(int ID, Models.Inventario value)    //Este ActualizarInventario NO actualiza la ID
        {
            var inventario = DbContext.Inventarios.Find(ID);
            if (inventario != null)
            {
                inventario.Cantidad = value.Cantidad;
                inventario.IDBodega = value.IDBodega;
                inventario.FechaIngreso = value.FechaIngreso;
                inventario.FechaVencimiento = value.FechaVencimiento;
                inventario.Tipo = value.Tipo;
                inventario.Precio = value.Precio;
                DbContext.SaveChanges();
            }
        }*/

        public void ActualizarInventario(int ID, Models.Inventario value)
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    // Obtener el inventario original
                    var inventario = DbContext.Inventarios.Find(ID);
                    if (inventario != null)
                    {
                        // Obtener todas las entidades dependientes
                        var pedidos = DbContext.Pedidos.Where(p => p.IDInventario == ID).ToList();

                        // Primero eliminar los pedidos relacionados con el inventario
                        DbContext.Pedidos.RemoveRange(pedidos);
                        DbContext.SaveChanges();

                        // Eliminar el inventario original
                        DbContext.Inventarios.Remove(inventario);
                        DbContext.SaveChanges();

                        // Crear un nuevo inventario con los datos actualizados
                        var nuevoInventario = new Models.Inventario
                        {
                            ID = value.ID,
                            Cantidad = value.Cantidad,
                            IDBodega = value.IDBodega,
                            FechaIngreso = value.FechaIngreso,
                            FechaVencimiento = value.FechaVencimiento,
                            Tipo = value.Tipo,
                            Precio = value.Precio
                        };
                        DbContext.Inventarios.Add(nuevoInventario);
                        DbContext.SaveChanges();

                        // Actualizar los pedidos con el nuevo ID del inventario y volver a insertarlos
                        foreach (var pedido in pedidos)
                        {
                            pedido.IDInventario = nuevoInventario.ID;
                            DbContext.Pedidos.Add(pedido);
                        }
                        DbContext.SaveChanges();

                        transaction.Commit();
                    }
                    else
                    {
                        throw new Exception("Inventario no encontrado.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    var innerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    throw new Exception($"{innerExceptionMessage}", ex);
                }
            }
        }

        /*public void ActualizarPedido(int ID, Models.Pedido value)
        {
            var pedido = DbContext.Pedidos.Find(ID);
            if (pedido != null)
            {
                EliminarMontoCliente(pedido.IDCliente, pedido);
                pedido.IDCliente = value.IDCliente;
                pedido.IDInventario = value.IDInventario;
                pedido.IDDireccion = value.IDDireccion;
                pedido.Cantidad = value.Cantidad;
                pedido.CostoSinIva = value.CostoSinIva;
                pedido.Costo = value.Costo;
                pedido.Fecha = value.Fecha;
                pedido.Estado = value.Estado;
                ActualizarMontoCliente(pedido.IDCliente, pedido);
                DbContext.SaveChanges();
            }
        }*/

        public void ActualizarPedido(int ID, Models.Pedido value)
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    // Obtener el pedido original
                    var pedido = DbContext.Pedidos.Find(ID);
                    if (pedido != null)
                    {
                        // Eliminar el pedido original
                        EliminarMontoCliente(pedido.IDCliente, pedido);
                        DbContext.Pedidos.Remove(pedido);
                        DbContext.SaveChanges();

                        // Crear un nuevo pedido con los datos actualizados
                        var nuevoPedido = new Models.Pedido
                        {
                            ID = value.ID,
                            IDCliente = value.IDCliente,
                            IDInventario = value.IDInventario,
                            IDDireccion = value.IDDireccion,
                            Cantidad = value.Cantidad,
                            CostoSinIva = value.CostoSinIva,
                            Costo = value.Costo,
                            Fecha = value.Fecha,
                            Estado = value.Estado
                        };
                        DbContext.Pedidos.Add(nuevoPedido);
                        ActualizarMontoCliente(nuevoPedido.IDCliente, nuevoPedido);
                        DbContext.SaveChanges();

                        transaction.Commit();
                    }
                    else
                    {
                        throw new Exception("Pedido no encontrado.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    var innerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    throw new Exception($"{innerExceptionMessage}", ex);
                }
            }
        }

        /*public void ActualizarDireccion(int ID, Models.Direccion value)
        {
            var pedido = DbContext.Direcciones.Find(ID);
            if (pedido != null)
            {
                pedido.IDCliente = value.IDCliente;
                pedido.Provincia = value.Provincia;
                pedido.Canton = value.Canton;
                pedido.Distrito = value.Distrito;
                pedido.PuntoWaze = value.PuntoWaze;
                pedido.URL = value.URL;
                pedido.EsCondominio = value.EsCondominio;
                DbContext.SaveChanges();
            }
        }*/

        public void ActualizarDireccion(int ID, Models.Direccion value)
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    // Obtener la dirección original
                    var direccion = DbContext.Direcciones.Find(ID);
                    if (direccion != null)
                    {
                        // Obtener todas las entidades dependientes
                        var pedidos = DbContext.Pedidos.Where(p => p.IDDireccion == ID).ToList();

                        // Primero eliminar los pedidos relacionados con la dirección
                        DbContext.Pedidos.RemoveRange(pedidos);
                        DbContext.SaveChanges();

                        // Eliminar la dirección original
                        DbContext.Direcciones.Remove(direccion);
                        DbContext.SaveChanges();

                        // Crear una nueva dirección con los datos actualizados
                        var nuevaDireccion = new Models.Direccion
                        {
                            ID = value.ID,
                            IDCliente = value.IDCliente,
                            Provincia = value.Provincia,
                            Canton = value.Canton,
                            Distrito = value.Distrito,
                            PuntoWaze = value.PuntoWaze,
                            URL = value.URL,
                            EsCondominio = value.EsCondominio
                        };
                        DbContext.Direcciones.Add(nuevaDireccion);
                        DbContext.SaveChanges();

                        // Actualizar los pedidos con el nuevo ID de la dirección y volver a insertarlos
                        foreach (var pedido in pedidos)
                        {
                            pedido.IDDireccion = nuevaDireccion.ID;
                            DbContext.Pedidos.Add(pedido);
                        }
                        DbContext.SaveChanges();

                        transaction.Commit();
                    }
                    else
                    {
                        throw new Exception("Dirección no encontrada.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    var innerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    throw new Exception($"{innerExceptionMessage}", ex);
                }
            }
        }

        //Operaciones de los reportes:
        public List<Models.Pedido> GetPedidosPorFecha(DateTime fecha)
        {
            List<Models.Pedido> PedidosPorFecha = new List<Models.Pedido>();
            var listaPedidos = GetListaPedidos();
            PedidosPorFecha = listaPedidos.Where(p => p.Fecha.Date == fecha.Date).ToList();

            return PedidosPorFecha;
        }

        public List<Models.Pedido> GetPedidosEnRango(DateTime fechaInicio, DateTime fechaFin)
        {
            List<Models.Pedido> RangoPedidos = new List<Models.Pedido>();
            var pedidos = GetListaPedidos();
            RangoPedidos = pedidos.Where(p => p.Fecha >= fechaInicio && p.Fecha <= fechaFin).ToList();
            return RangoPedidos;
        }


        public bool BodegaExiste(int IDBodega)
        {
            var inventario = GetListaInventario();
            var bodegaExiste = inventario.Any(i => i.IDBodega == IDBodega);
            return bodegaExiste;
        }

        public List<Models.Pedido> GetPedidosPorBodega(int idBodega)
        {
            var pedidos = GetListaPedidos();
            var inventarios = GetListaInventario();

            //IDBodega se encuentra en el inventario e IDInventario se encuentra en el pedido.
            var pedidosPorBodega = pedidos.Where(p => inventarios.Any(i => i.ID == p.IDInventario && i.IDBodega == idBodega)).ToList();

            return pedidosPorBodega;
        }

        public List<Models.Pedido> GetPedidosPorClienteYDia(int ID, DateTime fecha)
        {
            var listaPedidos = GetListaPedidos();
            var pedidosFiltrados = listaPedidos
                .Where(p => p.IDCliente == ID && p.Fecha.Date == fecha.Date)
                .ToList();
            return pedidosFiltrados;
        }

        public List<Models.Pedido> GetPedidosEnRangoPorCliente(int clienteId, DateTime fechaInicio, DateTime fechaFin)
        {
            var listaPedidos = GetListaPedidos();

            var pedidosEnRango = listaPedidos
                .Where(p => p.IDCliente == clienteId && p.Fecha >= fechaInicio && p.Fecha <= fechaFin)
                .ToList();

            return pedidosEnRango;
        }

        public List<Models.Pedido> GetPedidosPorCliente(int IDCliente)
        {
            var pedidos = GetListaPedidos();
            return pedidos.Where(p => p.IDCliente == IDCliente).ToList();
        }

        public bool HoraYMinutoEsValido(int Hora, int Minuto)
        {
            bool validar = false;
            if ((Hora <= 23 && Hora >= 0) && (Minuto <= 59 && Minuto >= 0))
            {
                validar = true;
            }
            return validar;
        }

        public List<Models.Pedido> GetPedidosPorClienteEnHoraMinuto(int IDCliente, int Hora, int Minuto)
        {
            var pedidosCliente = GetPedidosPorCliente(IDCliente);

            var pedidosFiltrados = pedidosCliente.Where(p =>
                p.Fecha.Hour == Hora && p.Fecha.Minute == Minuto
            ).ToList();

            return pedidosFiltrados;
        }

        private List<Models.Pedido> GetPedidosEnMes(int mes, int año)
        {
            var pedidos = GetListaPedidos();
            return pedidos
                .Where(p => p.Fecha.Month == mes && p.Fecha.Year == año)
                .ToList();
        }

        public bool MesEsValido(int mes)
        {
            bool validar = false;
            if (mes <= 12 && mes >= 1)
            {
                validar = true;
            }
            return validar;
        }

        public List<Models.Reporte> GetClientesConMasPedidosEnMes(int mes, int año)
        {
            var pedidosEnMes = GetPedidosEnMes(mes, año);
            var clientesConPedidos = pedidosEnMes
                .GroupBy(p => p.IDCliente)
                .Select(g => new Models.Reporte
                {
                    IDCliente = g.Key,
                    //IDPedidos = g.Select(p => p.ID).ToList(),
                    MontoTotalPedidos = g.Sum(p => p.Costo)
                })
                .OrderByDescending(c => c.MontoTotalPedidos)
                .ToList();

            return clientesConPedidos;
        }
    }
}

/*
 * Sobre el manejo de datos en los servicios:
 * En procesos REST, los datos se convierten a JSON antes de ser enviados al servidor, 
 * lo que permite una comunicación entre el cliente y el servidor. Al recibir los datos, 
 * el servidor los deserializa de JSON a objetos del lenguaje de programación para 
 * procesarlos, y luego serializa la respuesta de vuelta a JSON para enviarla al cliente.
 */
