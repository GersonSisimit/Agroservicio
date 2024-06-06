using Agroservicio.Models;
using Microsoft.AspNetCore.Mvc;

namespace Agroservicio.Controllers
{

    public class ClienteController : Controller
    {
        private readonly DbContext _contextDB;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ClienteController(DbContext context, IWebHostEnvironment hostEnvironment)
        {
            _contextDB = context;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            ViewBag.Clientes = _contextDB.Cliente.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult CrearCliente(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _contextDB.Cliente.Add(cliente);
                    _contextDB.SaveChanges();
                    TempData["CreacionExito"] = "Si";
                    TempData["Mensaje"] = "Creacion Exitosa";
                    return RedirectToAction("Index"); // Redirigir a otra acción después de guardar
                }
                catch (Exception Error)
                {
                    TempData["Error"] = "Si";
                    TempData["Mensaje"] = Error.Message;
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult EditarCliente(Cliente value)
        {

            var Existente = _contextDB.Cliente.Find(value.Id);
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

            return RedirectToAction("Index", "Cliente");
        }

        public IActionResult DireccionCliente()
        {
            ViewBag.DireccionesCliente = _contextDB.DireccionCliente.ToList();
            ViewBag.Clientes = _contextDB.Cliente.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult CrearDireccion(DireccionCliente value)
        {

            try
            {
                _contextDB.DireccionCliente.Add(value);
                _contextDB.SaveChanges();
                TempData["CreacionExito"] = "Si";
                TempData["Mensaje"] = "Creacion Exitosa";
            }
            catch (Exception Error)
            {
                TempData["Error"] = "Si";
                TempData["Mensaje"] = Error.Message;
            }

            return RedirectToAction("DireccionCliente");
        }
        [HttpPost]
        public IActionResult EditarDireccion(DireccionCliente value)
        {
            var Existente = _contextDB.DireccionCliente.Find(value.Id);
            if (Existente == null)
            {
                TempData["Error"] = "Si";
                TempData["Mensaje"] = "No se encontró el registro";
                return RedirectToAction("BaseProducto", "Producto");
            }
            if (string.IsNullOrWhiteSpace(value.Direccion))
            {
                TempData["Error"] = "Si";
                TempData["Mensaje"] = "No ingrese un valor en blanco";
                return RedirectToAction("BaseProducto", "Producto");
            }
            Existente.Direccion = value.Direccion;
            _contextDB.SaveChanges();
            TempData["CreacionExito"] = "Si";
            TempData["Mensaje"] = "Modificacion Exitosa";

            return RedirectToAction("Index", "Cliente");
        }
    }
}
