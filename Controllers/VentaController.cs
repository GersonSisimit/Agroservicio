using Agroservicio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Index()
        {
            // Verificar si existe la sesión con la clave "Productos"
            var sessionProductos = HttpContext.Session.GetString("Productos");

            var productosDetalles = new List<object>();
            if (string.IsNullOrEmpty(sessionProductos))
            {
                return View(productosDetalles);
            }

            var ProductosAgregados = JsonConvert.DeserializeObject<List<Producto>>(sessionProductos);
            if (ProductosAgregados != null)
            {
                foreach (var item in ProductosAgregados)
                {

                    //var detallesProducto = (from producto in _context.Producto
                    //                        join relacion in _context.DetalleProducto on producto.IdDetalleProducto equals relacion.Id
                    //                        join tipoProducto in _context.TipoProducto on relacion.IdTipoProducto equals tipoProducto.Id
                    //                        join subtipoProducto in _context.SubtipoProducto on relacion.IdSubtipoProducto equals subtipoProducto.Id
                    //                        join marca in _context.Marca on producto.IdMarca equals marca.Id
                    //                        join PresentacionProducto in _context.PresentacionProducto on producto.Id equals PresentacionProducto.IdProducto
                    //                        where PresentacionProducto.Id == item
                    //                        select new
                    //                        {
                    //                            PresentacionProducto.Id,
                    //                            PresentacionProducto.Precio,
                    //                            producto.Nombre,
                    //                            TipoProducto = tipoProducto.Nombre,
                    //                            SubtipoProducto = subtipoProducto.Nombre,
                    //                            Marca = marca.Nombre,
                    //                            RutaImagen = producto.RutaImagen,
                    //                            DatoCantidad = item.Existencia
                    //                        })
                    //                         .FirstOrDefault();

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
        [HttpPost]
        public JsonResult VaciarSesion()
        {
            try
            {
                // Vaciar la sesión
                HttpContext.Session.Remove("Productos");
                return Json(new { success = true, message = "Carrito vaciado" });
            }
            catch (System.Exception Error)
            {
                return Json(new { success = false, message = Error.Message });
            }
        }
    }
}
