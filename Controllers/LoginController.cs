using Agroservicio.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Agroservicio.Controllers
{
    public class LoginController : Controller
    {
        private readonly DbContext _contextDB;
        private readonly IWebHostEnvironment _hostEnvironment;

        public LoginController(DbContext context, IWebHostEnvironment hostEnvironment)
        {
            _contextDB = context;
            _hostEnvironment = hostEnvironment;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Usuario value)
        {

            HttpContext.Session.SetString("Message", "Hello MVC!");



            string message = HttpContext.Session.GetString("Message");

            return View();
        }

        [HttpPost]
        public IActionResult Registrarse(Usuario value)
        {
            try
            {
                value.Contraseña = EncriptarString(value.Contraseña);

                _contextDB.Usuario.Add(value);
                _contextDB.SaveChanges();

                TempData["Mensaje"] = "Registro exitoso";

            }
            catch (Exception error)
            {
                TempData["Mensaje"] = error.Message;
            }
            return RedirectToAction("Login", "Login");

        }

        public static string EncriptarString(string texto)
        {

            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}
