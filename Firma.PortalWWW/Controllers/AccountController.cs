using Firma.Data.Data;
using Firma.Data.Data.Customers;
using Firma.PortalWWW.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Firma.PortalWWW.Controllers
{
    public class AccountController : Controller
    {
        private readonly FirmaContext _context;

        public AccountController(FirmaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Użytkownik o tym adresie email już istnieje.");
                    return View(model);
                }

                // Tworzę nowego użytkownika, używając wszystkich pól z Twojego ViewModelu
                var user = new User
                {
                    Email = model.Email,
                    HashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    Name = model.Name,       // <-- Używam pola Name
                    Phone = model.Phone,     // <-- Używam pola Phone
                    Role = UserRole.User,
                    RegistrationDate = DateTime.Now
                };

                _context.User.Add(user);
                await _context.SaveChangesAsync();

                // Tworzę tymczasowy LoginViewModel, aby automatycznie zalogować użytkownika
                var loginViewModel = new LoginViewModel
                {
                    Email = model.Email,
                    Password = model.Password,
                    RememberMe = true
                };
                // Wywołuję akcję Login
                await Login(loginViewModel);

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.HashedPassword))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Role, user.Role.ToString()),
                        new Claim("UserId", user.IdUser.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties { IsPersistent = model.RememberMe };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Nieprawidłowa próba logowania.");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}