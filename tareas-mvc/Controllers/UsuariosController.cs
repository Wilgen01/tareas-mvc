using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tareas_mvc.Models;
using tareas_mvc.Servicios;

namespace tareas_mvc.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ApplicationDbContext context;

        public UsuariosController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        [AllowAnonymous]
        public IActionResult Registro()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(RegistroViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = new IdentityUser() { Email = model.Email, UserName = model.Email};

            var resultado = await userManager.CreateAsync(usuario, password: model.Password);

            if (resultado.Succeeded)
            {
                await signInManager.SignInAsync(usuario, isPersistent: true);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(RegistroViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var resultado = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (resultado.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.TryAddModelError(String.Empty, "Credenciales incorrectas");
                return View(model);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("login", "usuarios");
        }

        [AllowAnonymous]
        [HttpGet]
        public ChallengeResult LoginExterno(string proovedor, string urlRetorno = null)
        {
            var urlRedirection = Url.Action("RegistrarUsuarioExterno", values: new {urlRetorno});
            var propiedades  = signInManager.ConfigureExternalAuthenticationProperties(proovedor, urlRedirection);
            return new ChallengeResult(proovedor, propiedades);
        }

        [HttpGet]
        public async Task<IActionResult> Listado(string mensaje = null, string error = null)
        {
            var usuarios = await context.Users.Select(u => new UsuarioViewModel() { Email = u.Email}).ToListAsync();

            var modelo = new UsuariosListadoViewModel();
            modelo.Usuarios = usuarios;
            modelo.Mensaje = mensaje;
            modelo.Error = error;
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> HacerAdmin(string email)
        {
            var usuario = await context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();

            if (usuario is null)
            {
                return NotFound();
            }

            var belongToAdmin = await userManager.IsInRoleAsync(usuario, Constantes.RolAdmin);
            if (belongToAdmin)
            {
                return RedirectToAction("Listado", routeValues: new { error = email + " ya pertenece a este rol" });
            }

            await userManager.AddToRoleAsync(usuario, Constantes.RolAdmin);

            return RedirectToAction("Listado", routeValues: new {mensaje = "Rol asignado correctamente a " + email});
        }

        [HttpPost]
        public async Task<IActionResult> RemoverAdmin(string email)
        {
            var usuario = await context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();

            if (usuario is null)
            {
                return NotFound();
            }

            var belongToAdmin = await userManager.IsInRoleAsync(usuario, Constantes.RolAdmin);
            if (!belongToAdmin)
            {
                return RedirectToAction("Listado", routeValues: new { error = email + " no pertenece a este rol" });
            }

            await userManager.RemoveFromRoleAsync(usuario, Constantes.RolAdmin);

            return RedirectToAction("Listado", routeValues: new { mensaje = "Rol removido correctamente a " + email });
        }

    }
}
