using Authentication.Models;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace Authentication.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                if (model.UserName == model.Password)
                {
                    var claims = new List<Claim>
                    {
                        new Claim("sub", model.UserName),
                        new Claim("name", "bob"),
                        new Claim("email", "bob@smith.com")
                    };

                    var id = new ClaimsIdentity(claims, "local", "name", "role");
                    Context.Authentication.SignIn("Cookies", new ClaimsPrincipal(id));
                    
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }

        public IActionResult Logoff()
        {
            Context.Authentication.SignOut();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private IActionResult RedirectToLocal(string returnUrl)
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