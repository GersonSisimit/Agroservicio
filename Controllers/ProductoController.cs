using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agroservicio.Controllers
{
    public class ProductoController : Controller
    {
        public ActionResult Crear()
        {
            return View();
        }
    }
}
