using Agroservicio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Agroservicio.Controllers
{
    public class ProductoController : Controller
    {
        private readonly DbContext _contextDB;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductoController(DbContext context, IWebHostEnvironment hostEnvironment)
        {
            _contextDB = context;
            _hostEnvironment = hostEnvironment;
        }

        public ActionResult Crear()
        {

            return View();
        }
        public ActionResult ControlDetalle()
        {
            //pasar por lista los registros necesarios paran crear la base de un producto

            ViewBag.TipoProductoList = _contextDB.TipoProducto.ToList();
            ViewBag.MarcaList = _contextDB.Marca.ToList();
            ViewBag.GrupoTipoProductoList = _contextDB.GrupoTipoProducto.ToList();


            return View();
        }
        #region GrupoTipoProducto
        public ActionResult GrupoTipoProducto()
        {
            ViewBag.GrupoTipoProductoList = _contextDB.GrupoTipoProducto.ToList();

            return View();
        }
        [HttpPost]
        public ActionResult CrearGrupoTipoProducto(GrupoTipoProducto value)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _contextDB.GrupoTipoProducto.Add(value);
                    _contextDB.SaveChanges();
                    TempData["CreacionExito"] = "Si";
                    TempData["Mensaje"] = "Creacion Exitosa";
                }
                catch (Exception Error)
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = Error.Message;
                }
            }
            return RedirectToAction("GrupoTipoProducto", "Producto");
        }
        [HttpPost]
        public IActionResult EditarGrupoTipoProducto(GrupoTipoProducto value)
        {
            if (ModelState.IsValid)
            {
                var Existente = _contextDB.GrupoTipoProducto.Find(value.Id);
                if (Existente == null)
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = "No se encontró el registro";
                    return RedirectToAction("ControlDetalle", "Producto");
                }
                if (string.IsNullOrWhiteSpace(value.Nombre))
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = "No ingrese un nombre en blanco";
                    return RedirectToAction("ControlDetalle", "Producto");
                }
                Existente.Nombre = value.Nombre;
                _contextDB.SaveChanges();
                TempData["CreacionExito"] = "Si";
                TempData["Mensaje"] = "Modificacion Exitosa";
            }
            return RedirectToAction("GrupoTipoProducto", "Producto");
        }
        #endregion

        #region Empaque Producto

        public ActionResult EmpaqueProducto()
        {
            ViewBag.EmpaqueProducto = _contextDB.EmpaqueProducto.ToList();

            return View();
        }
        [HttpPost]
        public ActionResult CrearEmpaqueProducto(EmpaqueProducto value)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _contextDB.EmpaqueProducto.Add(value);
                    _contextDB.SaveChanges();
                    TempData["CreacionExito"] = "Si";
                    TempData["Mensaje"] = "Creacion Exitosa";
                }
                catch (Exception Error)
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = Error.Message;
                }
            }
            return RedirectToAction("EmpaqueProducto", "Producto");
        }
        [HttpPost]
        public IActionResult EditarEmpaqueProducto(EmpaqueProducto value)
        {
            if (ModelState.IsValid)
            {
                var Existente = _contextDB.EmpaqueProducto.Find(value.Id);
                if (Existente == null)
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = "No se encontró el registro";
                    return RedirectToAction("EmpaqueProducto", "Producto");
                }
                if (string.IsNullOrWhiteSpace(value.Nombre))
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = "No ingrese un nombre en blanco";
                    return RedirectToAction("EmpaqueProducto", "Producto");
                }
                Existente.Nombre = value.Nombre;
                _contextDB.SaveChanges();
                TempData["CreacionExito"] = "Si";
                TempData["Mensaje"] = "Modificacion Exitosa";
            }
            return RedirectToAction("EmpaqueProducto", "Producto");
        }

        #endregion

        #region Marca
        public ActionResult Marca()
        {
            ViewBag.Marca = _contextDB.Marca.ToList();

            return View();
        }
        [HttpPost]
        public ActionResult CrearMarca(Marca value)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _contextDB.Marca.Add(value);
                    _contextDB.SaveChanges();
                    TempData["CreacionExito"] = "Si";
                    TempData["Mensaje"] = "Creacion Exitosa";
                }
                catch (Exception Error)
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = Error.Message;
                }
            }
            return RedirectToAction("Marca", "Producto");
        }
        [HttpPost]
        public IActionResult EditarMarca(Marca value)
        {
            if (ModelState.IsValid)
            {
                var Existente = _contextDB.Marca.Find(value.Id);
                if (Existente == null)
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = "No se encontró el registro";
                    return RedirectToAction("Marca", "Producto");
                }
                if (string.IsNullOrWhiteSpace(value.Nombre))
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = "No ingrese un nombre en blanco";
                    return RedirectToAction("Marca", "Producto");
                }
                Existente.Nombre = value.Nombre;
                _contextDB.SaveChanges();
                TempData["CreacionExito"] = "Si";
                TempData["Mensaje"] = "Modificacion Exitosa";
            }
            return RedirectToAction("Marca", "Producto");
        }
        #endregion

        #region Tipo producto

        public ActionResult TipoProducto()
        {
            ViewBag.TipoProducto = _contextDB.TipoProducto.ToList();
            ViewBag.GrupoTipoProducto = _contextDB.GrupoTipoProducto.ToList();

            return View();
        }
        [HttpPost]
        public ActionResult CrearTipoProducto(TipoProducto value)
        {

            try
            {
                _contextDB.TipoProducto.Add(value);
                _contextDB.SaveChanges();
                TempData["CreacionExito"] = "Si";
                TempData["Mensaje"] = "Creacion Exitosa";
            }
            catch (Exception Error)
            {
                TempData["Error"] = "Si";
                TempData["Mensaje"] = Error.Message;
            }

            return RedirectToAction("TipoProducto", "Producto");
        }
        [HttpPost]
        public IActionResult EditarTipoProducto(TipoProducto value)
        {

            var Existente = _contextDB.TipoProducto.Find(value.Id);
            if (Existente == null)
            {
                TempData["Error"] = "Si";
                TempData["Mensaje"] = "No se encontró el registro";
                return RedirectToAction("TipoProducto", "Producto");
            }
            if (string.IsNullOrWhiteSpace(value.Nombre))
            {
                TempData["Error"] = "Si";
                TempData["Mensaje"] = "No ingrese un nombre en blanco";
                return RedirectToAction("TipoProducto", "Producto");
            }
            Existente.Nombre = value.Nombre;
            _contextDB.SaveChanges();
            TempData["CreacionExito"] = "Si";
            TempData["Mensaje"] = "Modificacion Exitosa";

            return RedirectToAction("TipoProducto", "Producto");
        }

        #endregion

        #region BaseProducto
        public ActionResult BaseProducto()
        {
            ViewBag.BaseProducto = _contextDB.BaseProducto.ToList();
            ViewBag.TipoProducto = _contextDB.TipoProducto.ToList();
            ViewBag.Marca = _contextDB.Marca.ToList();


            return View();
        }
        [HttpPost]
        public ActionResult CrearBaseProducto(BaseProducto value)
        {

            try
            {
                _contextDB.BaseProducto.Add(value);
                _contextDB.SaveChanges();
                TempData["CreacionExito"] = "Si";
                TempData["Mensaje"] = "Creacion Exitosa";
            }
            catch (Exception Error)
            {
                TempData["Error"] = "Si";
                TempData["Mensaje"] = Error.Message;
            }

            return RedirectToAction("BaseProducto", "Producto");
        }
        [HttpPost]
        public IActionResult EditarBaseProducto(BaseProducto value)
        {

            var Existente = _contextDB.BaseProducto.Find(value.Id);
            if (Existente == null)
            {
                TempData["Error"] = "Si";
                TempData["Mensaje"] = "No se encontró el registro";
                return RedirectToAction("BaseProducto", "Producto");
            }
            if (string.IsNullOrWhiteSpace(value.Nombre))
            {
                TempData["Error"] = "Si";
                TempData["Mensaje"] = "No ingrese un nombre en blanco";
                return RedirectToAction("BaseProducto", "Producto");
            }
            Existente.Nombre = value.Nombre;
            _contextDB.SaveChanges();
            TempData["CreacionExito"] = "Si";
            TempData["Mensaje"] = "Modificacion Exitosa";

            return RedirectToAction("BaseProducto", "Producto");
        }
        #endregion

        #region Producto 
        /// <summary>
        /// Vista para mostrar las diversas presentaciones que puede tener un producto
        /// </summary>
        /// <param name="idProductoBase">Id producto base</param>
        /// <returns></returns>
        public ActionResult PresentacionProducto(int IdBaseProducto)
        {
            var BaseProducto = _contextDB.BaseProducto.Where(BaseProducto => BaseProducto.Id == IdBaseProducto).FirstOrDefault();
            if (BaseProducto != null)
            {
                var Marca = _contextDB.Marca.Where(marca => marca.Id == BaseProducto.IdMarca).FirstOrDefault();
                var TipoProducto = _contextDB.TipoProducto.Where(TipoProducto => TipoProducto.Id == BaseProducto.IdTipoProducto).FirstOrDefault();

                var Grupo = _contextDB.GrupoTipoProducto.Where(x => x.Id == TipoProducto.IdGrupoTipoProducto).FirstOrDefault();

                var EmpaqueProductoLista = _contextDB.EmpaqueProducto.ToList();

                ViewBag.ProductoBaseNombre = BaseProducto.Nombre;
                ViewBag.IdBaseProducto = IdBaseProducto;
                ViewBag.Marca = Marca.Nombre;
                ViewBag.TipoProducto = TipoProducto.Nombre;
                ViewBag.Grupo = Grupo.Nombre;

                ViewBag.EmpaqueProductoLista = EmpaqueProductoLista;
            }
            ViewBag.PresentacionesProducto = _contextDB.Producto.Where(Producto => Producto.IdBaseProducto == IdBaseProducto).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult CrearProducto(Producto value)
        {

            try
            {
                var Busqueda = _contextDB.Producto.Where(x => x.IdBaseProducto == value.IdBaseProducto && x.IdEmpaqueProducto == value.IdEmpaqueProducto).ToList();

                if (Busqueda.Count != 0)
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = "Ya existe una presentacion creada";
                    return RedirectToAction("PresentacionProducto", "Producto", new { IdBaseProducto = value.IdBaseProducto });

                }

                value.RutaImagen = "";
                _contextDB.Producto.Add(value);
                _contextDB.SaveChanges();
                TempData["CreacionExito"] = "Si";
                TempData["Mensaje"] = "Creacion Exitosa";
            }
            catch (Exception Error)
            {
                TempData["Error"] = "Si";
                TempData["Mensaje"] = Error.Message;
            }

            return RedirectToAction("PresentacionProducto", "Producto",new { IdBaseProducto  = value.IdBaseProducto });
        }
        #endregion
    }
}
