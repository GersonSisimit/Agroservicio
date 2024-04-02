using Agroservicio.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft;
using Agroservicio.Migrations;
using Newtonsoft.Json;


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
        public IActionResult Login2(Usuario value)
        {
            try
            {
                var UsuarioNombre = _contextDB.Usuario.
                Where(x => x.Nombre == value.Nombre)
                .FirstOrDefault();

                var Usuario = _contextDB.Usuario.
                Where(x => x.Nombre == value.Nombre && x.Contraseña == EncriptarString(value.Contraseña))
                .FirstOrDefault();

                if (Usuario != null)
                {

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Resultado"] = "Error";
                    if (UsuarioNombre != null)
                    {
                        TempData["Mensaje"] = "Contraseña incorrecta";
                    }
                    else
                    {
                        TempData["Mensaje"] = "Usuario no encontrado";
                    }
                }
            }
            catch (Exception Error)
            {
                TempData["Resultado"] = "Error";
                TempData["Mensaje"] = Error.Message;
            }
            return View();






            //HttpContext.Session.SetString("Message", "Hello MVC!");



            //string message = HttpContext.Session.GetString("Message");


        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home"); // Redirigir a la página principal
        }



        [HttpPost]
        public async Task<IActionResult> Login(Usuario value)
        {
            if (!ModelState.IsValid)
            {
                return View(); // Mantener errores de validación
            }


            try
            {
                var UsuarioNombre = _contextDB.Usuario.
                Where(x => x.Nombre == value.Nombre)
                .FirstOrDefault();

                var Usuario = _contextDB.Usuario.
                Where(x => x.Nombre == value.Nombre && x.Contraseña == EncriptarString(value.Contraseña))
                .FirstOrDefault();

                if (Usuario != null)
                {

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, value.Nombre),
                        new Claim("IdUsuario", value.Id.ToString()),
                        // Agregar claims adicionales según sea necesario
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true // Opcional: Permitir inicios de sesión persistentes (cookies en las sesiones del navegador)
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), authProperties);

                    //Guardando la informacion del usuario 

                    var json = JsonConvert.SerializeObject(Usuario);
                    HttpContext.Session.SetString("Usuario", json);


                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Resultado"] = "Error";
                    if (UsuarioNombre != null)
                    {
                        TempData["Mensaje"] = "Contraseña incorrecta";
                    }
                    else
                    {
                        TempData["Mensaje"] = "Usuario no encontrado";
                    }
                }
            }
            catch (Exception Error)
            {
                TempData["Resultado"] = "Error";
                TempData["Mensaje"] = Error.Message;
            }
            return View(); // Mostrar el formulario de inicio de sesión con un mensaje de error
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
