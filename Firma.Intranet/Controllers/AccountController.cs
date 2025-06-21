using Firma.Data.Data;
using Firma.Intranet.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Firma.Intranet.Controllers
{
    public class AccountController : Controller
    {
        private readonly FirmaContext _context;

        public AccountController(FirmaContext context)
        {
            _context = context;
        }

        // Wyświetla formularz logowania
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
         
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // Przetwarza dane logowania
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.HashedPassword))
                {
                    // Tworzę listę "oświadczeń" (claims) o użytkowniku
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.Role, user.Role.ToString()),
                        new Claim("UserId", user.IdUser.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe, // "Zapamiętaj mnie"
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
                    };

                    // Loguję użytkownika (tworzę ciasteczko autoryzacyjne)
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    // Przekierowuję na stronę, na którą chciał wejść, lub na główną
                    return RedirectToLocal(returnUrl);
                }
                ModelState.AddModelError(string.Empty, "Nieprawidłowa próba logowania.");
            }
            return View(model);
        }

        // Wylogowuje użytkownika
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // Metoda pomocnicza do bezpiecznego przekierowania
        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}