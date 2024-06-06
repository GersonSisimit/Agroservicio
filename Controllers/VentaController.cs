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
            if (string.IsNullOrEmpty(sessionProductos))
            {
                // Si no existe la sesión, redirigir o manejarlo adecuadamente
                return View(productosDetalles);
            }

            // Deserializar el string en una lista de enteros
            List<int> IdproductosEnSesion = JsonConvert.DeserializeObject<List<int>>(sessionProductos);

            var Cantidad = JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("Cantidad"));

            int indice = 0;
            //foreach (var item in IdproductosEnSesion)
            //{
            //    var DatoCantidad = Cantidad[indice];
            //    var detallesProducto = (from producto in _context.Producto
            //                            join relacion in _context.DetalleProducto on producto.IdDetalleProducto equals relacion.Id
            //                            join tipoProducto in _context.TipoProducto on relacion.IdTipoProducto equals tipoProducto.Id
            //                            join subtipoProducto in _context.SubtipoProducto on relacion.IdSubtipoProducto equals subtipoProducto.Id
            //                            join marca in _context.Marca on producto.IdMarca equals marca.Id
            //                            join PresentacionProducto in _context.PresentacionProducto on producto.Id equals PresentacionProducto.IdProducto
            //                            where PresentacionProducto.Id == item
            //                            select new
            //                            {
            //                                PresentacionProducto.Id,
            //                                PresentacionProducto.Precio,
            //                                producto.Nombre,
            //                                TipoProducto = tipoProducto.Nombre,
            //                                SubtipoProducto = subtipoProducto.Nombre,
            //                                Marca = marca.Nombre,
            //                                RutaImagen = producto.RutaImagen,
            //                                DatoCantidad
            //                            })
            //                             .FirstOrDefault();

            //    if (detallesProducto != null)
            //    {
            //        productosDetalles.Add(detallesProducto);
            //    }
            //    indice++;
            //}
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
    }
}
