using Agroservicio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace Agroservicio.Controllers
{
    public class VentaController : Controller
    {
        private readonly DbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public VentaController(DbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }


        public IActionResult SeleccionarCliente(int id)
        {
            HttpContext.Session.SetString("ClienteCarrito", JsonConvert.SerializeObject(id));

            return RedirectToAction("Index");
        }
        public IActionResult Index()
        {
            var productosDetalles = new List<object>();
            #region Validacion Cliente Seleccionado

            var ClienteCarrito = HttpContext.Session.GetString("ClienteCarrito");
            if (ClienteCarrito == null)
            {
                ViewBag.Clientes = _context.Cliente.ToList();
                return View(productosDetalles);
            }
            var IdClienteCarrito = JsonConvert.DeserializeObject<int>(ClienteCarrito);

            ViewBag.ClienteCarrito = _context.Cliente.Find(IdClienteCarrito);
            ViewBag.ListaDireccionCliente = _context.DireccionCliente.Where(x => x.IdCliente == IdClienteCarrito).ToList();

            #endregion

            #region Validacion Productos Agregados
            // Verificar si existe la sesión con la clave "Productos"
            var sessionProductos = HttpContext.Session.GetString("Productos");


            if (string.IsNullOrEmpty(sessionProductos))
            {
                return View(productosDetalles);
            }

            var ProductosAgregados = JsonConvert.DeserializeObject<List<Producto>>(sessionProductos);
            if (ProductosAgregados != null)
            {
                foreach (var item in ProductosAgregados)
                {
                    var detallesProducto = (from Producto in _context.Producto
                                            join BaseProducto in _context.BaseProducto on Producto.IdBaseProducto equals BaseProducto.Id
                                            join Empaque in _context.EmpaqueProducto on Producto.IdEmpaqueProducto equals Empaque.Id
                                            join Marca in _context.Marca on BaseProducto.IdMarca equals Marca.Id
                                            join TipoProducto in _context.TipoProducto on BaseProducto.IdTipoProducto equals TipoProducto.Id
                                            join GrupoTipoProducto in _context.GrupoTipoProducto on TipoProducto.IdGrupoTipoProducto equals GrupoTipoProducto.Id
                                            where Producto.Id == item.Id
                                            select new
                                            {
                                                Precio = Producto.Precio,
                                                RutaImagen = Producto.RutaImagen,
                                                ProductoExistencia = Producto.Existencia,
                                                ProductoId = Producto.Id,
                                                Nombre = BaseProducto.Nombre,
                                                EmpaqueNombre = Empaque.Nombre,
                                                Marca = Marca.Nombre, // Añadiendo este si es necesario
                                                TipoProducto = TipoProducto.Nombre, // Añadiendo este si es necesario
                                                GrupoTipoProducto = GrupoTipoProducto.Nombre,
                                                Cantidad = item.Existencia

                                            })
                                          .FirstOrDefault();

                    if (detallesProducto != null)
                    {
                        productosDetalles.Add(detallesProducto);
                    }
                }
            }
            #endregion
            return View(productosDetalles);
        }


        [HttpPost]
        public IActionResult BuscarProducto(string busqueda)
        {
            var productosDetalles = new List<object>();

            var Busqueda = _context.BaseProducto.Where(x => x.Nombre.Contains(busqueda)).ToList();

            foreach (var item in Busqueda)
            {
                var detalleProducto = (from Producto in _context.Producto
                                       join BaseProducto in _context.BaseProducto on Producto.IdBaseProducto equals BaseProducto.Id
                                       join Empaque in _context.EmpaqueProducto on Producto.IdEmpaqueProducto equals Empaque.Id
                                       join Marca in _context.Marca on BaseProducto.IdMarca equals Marca.Id
                                       join TipoProducto in _context.TipoProducto on BaseProducto.IdTipoProducto equals TipoProducto.Id
                                       join GrupoTipoProducto in _context.GrupoTipoProducto on TipoProducto.IdGrupoTipoProducto equals GrupoTipoProducto.Id
                                       where BaseProducto.Id == item.Id
                                       select new
                                       {
                                           ProductoPrecio = Producto.Precio,
                                           RutaImagen = Producto.RutaImagen,
                                           ProductoExistencia = Producto.Existencia,
                                           ProductoId = Producto.Id,
                                           BaseProductoNombre = BaseProducto.Nombre,
                                           EmpaqueNombre = Empaque.Nombre,
                                           MarcaNombre = Marca.Nombre, // Añadiendo este si es necesario
                                           TipoProductoNombre = TipoProducto.Nombre, // Añadiendo este si es necesario
                                           GrupoTipoProductoNombre = GrupoTipoProducto.Nombre // Añadiendo este si es necesario
                                       })
                                       .ToList();

                if (detalleProducto != null)
                {
                    productosDetalles.AddRange(detalleProducto);
                }
            }

            return PartialView("_ResultadosBusquedaPartial", productosDetalles);
        }
        [HttpPost]
        public JsonResult SeleccionarDireccion(int IdDireccion)
        {
            HttpContext.Session.SetString("IdDireccionCliente", JsonConvert.SerializeObject(IdDireccion));

            return Json(new { success = true, message = "Dirección seleccionada" });
        }


        [HttpPost]
        public IActionResult InsertarProducto(List<Producto> DatosSeleccionados)
        {
            try
            {
                // Obtener la lista de productos existente de la sesión
                var sessionProductos = HttpContext.Session.GetString("Productos");

                if (sessionProductos == null)
                {
                    HttpContext.Session.SetString("Productos", JsonConvert.SerializeObject(DatosSeleccionados));
                    return Json(new { Message = "Producto agregado", success = true });
                }

                var productosAgregadosAnterior = JsonConvert.DeserializeObject<List<Producto>>(sessionProductos);
                productosAgregadosAnterior.AddRange(DatosSeleccionados);
                HttpContext.Session.SetString("Productos", JsonConvert.SerializeObject(productosAgregadosAnterior));
                return Json(new { Message = "Producto agregado", success = true });
            }
            catch (System.Exception Error)
            {
                return Json(new { Error = Error.Message, success = false });
            }
        }
        public IActionResult RegistrarVenta()
        {
            //Obtener los datos guardados de las sessiones generadas
            var IdDireccionCliente = HttpContext.Session.GetString("IdDireccionCliente");

            if (IdDireccionCliente == null)
            {
                TempData["Error"] = "Si";
                TempData["Mensaje"] = "Seleccione una direccion del cliente";
                return RedirectToAction("Index");
            }
            int IdDireccion = JsonConvert.DeserializeObject<int>(IdDireccionCliente);

            var sessionProductos = HttpContext.Session.GetString("Productos");
            var ClienteCarrito = HttpContext.Session.GetString("ClienteCarrito");

            if (sessionProductos != null && ClienteCarrito != null) {
                var ProductosAgregados = JsonConvert.DeserializeObject<List<Producto>>(sessionProductos);
                var IdClienteCarrito = JsonConvert.DeserializeObject<int>(ClienteCarrito);


                var productosDetalles = new List<object>();
                if (ProductosAgregados != null)
                {
                    foreach (var item in ProductosAgregados)
                    {
                        var detallesProducto = (from Producto in _context.Producto
                                                join BaseProducto in _context.BaseProducto on Producto.IdBaseProducto equals BaseProducto.Id
                                                join Empaque in _context.EmpaqueProducto on Producto.IdEmpaqueProducto equals Empaque.Id
                                                join Marca in _context.Marca on BaseProducto.IdMarca equals Marca.Id
                                                join TipoProducto in _context.TipoProducto on BaseProducto.IdTipoProducto equals TipoProducto.Id
                                                join GrupoTipoProducto in _context.GrupoTipoProducto on TipoProducto.IdGrupoTipoProducto equals GrupoTipoProducto.Id
                                                where Producto.Id == item.Id
                                                select new
                                                {
                                                    ProductoId = Producto.Id,
                                                    Producto.Precio,
                                                    Cantidad = item.Existencia,
                                                    SubTotal = Producto.Precio * item.Existencia
                                                })
                                              .FirstOrDefault();

                        if (detallesProducto != null)
                        {
                            productosDetalles.Add(detallesProducto);
                        }
                    }
                }


                // Obtener el valor del claim "IdUsuario" y registrar quien ralizó la factura
                var idUsuarioClaim = User.FindFirst("IdUsuario")?.Value;
                Factura RegistroFactura = new Factura();
                if (idUsuarioClaim != null)
                {
                    RegistroFactura.IdUsuario = int.Parse(idUsuarioClaim);
                }
                RegistroFactura.IdCliente = IdClienteCarrito;
                RegistroFactura.Fecha = DateTime.Now;
                RegistroFactura.IdDireccionCliente = IdDireccion;
               

                foreach (var detalles in productosDetalles)
                {
                    var tipo = detalles.GetType();
                    var subTotal = tipo.GetProperty("SubTotal").GetValue(detalles, null);

                    RegistroFactura.Total +=(double) subTotal;
                }

                var NuevoRegistro = _context.Factura.Add(RegistroFactura);
                _context.SaveChanges();

                foreach (var detalles in productosDetalles)
                {
                    var tipo = detalles.GetType();
                    var productoId = tipo.GetProperty("ProductoId").GetValue(detalles, null);
                    var precio = tipo.GetProperty("Precio").GetValue(detalles, null);
                    var cantidad = tipo.GetProperty("Cantidad").GetValue(detalles, null);
                    var subTotal = tipo.GetProperty("SubTotal").GetValue(detalles, null);

                    LineaFactura DetalleFactura = new LineaFactura();
                    DetalleFactura.IdFactura = NuevoRegistro.Entity.Id;
                    DetalleFactura.IdProducto = (int)productoId;
                    DetalleFactura.Cantidad = (double)cantidad;
                    DetalleFactura.Precio = (double)precio;
                    DetalleFactura.SubTotal = (double)subTotal;


                    try
                    {
                        RebajarExistenciaProducto(DetalleFactura.IdProducto, DetalleFactura.Cantidad);
                    }
                    catch
                    {
                    }
                   
                    _context.LineaFactura.Add(DetalleFactura);
                    _context.SaveChanges();
                }
                TempData["CreacionExito"] = "Si";
                TempData["Mensaje"] = "Venta generada exitosamente";
                // Vaciar la sesión de cliente y carrito
                EliminarSesionCarrito();
            }
            else
            {
                TempData["Error"] = "Si";
                TempData["Mensaje"] = "Error en carga de datos";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult VaciarSesion()
        {
            try
            {
                EliminarSesionCarrito();
                return Json(new { success = true, message = "Carrito vaciado" });
            }
            catch (System.Exception Error)
            {
                return Json(new { success = false, message = Error.Message });
            }
        }

        public void RebajarExistenciaProducto(int IdProducto,double CantidadRebaja)
        {
            var Producto = _context.Producto.Find(IdProducto);
            Producto.Existencia -=CantidadRebaja;
            _context.SaveChanges();
        }

        public void EliminarSesionCarrito()
        {
            // Vaciar la sesión de cliente y carrito
            HttpContext.Session.Remove("ClienteCarrito");
            HttpContext.Session.Remove("IdDireccionCliente");
            HttpContext.Session.Remove("Productos");
        }

        public IActionResult ReporteVentas()
        {
            var facturas = _context.Factura
       .Include(f => f.DireccionCliente)
       .ToList();

            var lineasFactura = _context.LineaFactura
                .Include(lf => lf.Producto)
                    .ThenInclude(p => p.BaseProducto)
                .ToList();

            var ventasPorCliente = facturas
                .GroupBy(f => f.IdCliente)
                .Select(g => new
                {
                    IdCliente = g.Key,
                    TotalVentas = g.Sum(f => f.Total)
                })
                .ToList();

            ViewBag.LineasFactura = lineasFactura;
            ViewBag.VentasPorCliente = ventasPorCliente;
            return View(facturas);
        }


    }
}
