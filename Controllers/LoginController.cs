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
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;


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

            // Obtener el token de reCAPTCHA del formulario
            var captchaResponse = Request.Form["g-recaptcha-response"];
            var secretKey = "6LffBl0qAAAAAM0srWEfYgZzsOppCbmCwmX7WfxY";

            // Validar el token de reCAPTCHA con la API de Google
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={captchaResponse}", null);
            var jsonResult = await response.Content.ReadAsStringAsync();
            dynamic result = JObject.Parse(jsonResult);

            // Verificar si el captcha fue exitoso
            if (result.success != true)
            {
                TempData["Resultado"] = "Error";
                TempData["Mensaje"] = "Verificación de CAPTCHA fallida.";
                return View(); // Mostrar el formulario de inicio de sesión con el mensaje de error
            }

            try
            {
                var UsuarioNombre = _contextDB.Usuario
                    .Where(x => x.Nombre == value.Nombre)
                    .FirstOrDefault();

                var Usuario = _contextDB.Usuario
                    .Where(x => x.Nombre == value.Nombre && x.Contraseña == EncriptarString(value.Contraseña))
                    .FirstOrDefault();

                if (Usuario != null)
                {
                    if (!Usuario.Activo)
                    {
                        TempData["Resultado"] = "Error";
                        TempData["Mensaje"] = "Su usuario esta inactivo, contacte a su administrador";
                        return RedirectToAction("Index", "Home");
                    }
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, value.Nombre),
                        new Claim("IdUsuario", value.Id.ToString()),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true // Mantener la sesión iniciada
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), authProperties);

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

            return View(); // Mostrar el formulario de inicio de sesión con el mensaje de error
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
