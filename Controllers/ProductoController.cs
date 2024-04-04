using Agroservicio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [HttpPost]
        public ActionResult CrearGrupoTipoProducto(GrupoTipoProducto value )
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
            return RedirectToAction("ControlDetalle", "Producto");
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
            return RedirectToAction("ControlDetalle", "Producto");
        }
    }
}
