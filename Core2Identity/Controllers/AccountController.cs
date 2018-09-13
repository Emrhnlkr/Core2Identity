using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core2Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Core2Identity.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signinManager;


        public AccountController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signinManager)
        {
            userManager = _userManager;
            signinManager = _signinManager;
        }




        [AllowAnonymous] /*Busayfayı giriş yapmauan kişi görecek*/
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous] /*Busayfayı giriş yapmauan kişi görecek*/
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string returnUrl,LoginModel model)
        {
            if (ModelState.IsValid)
            {
               


                var user = await userManager.FindByEmailAsync(model.Email);
                if (user!=null)
                {
                    await signinManager.SignOutAsync();
                    var result = await signinManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/");

                    }
                }
                ModelState.AddModelError("email", "Şifre veya e-mail hatalı");



            }

           
            return View(model);
        }

        public async Task<IActionResult> Logout()

        {
            await signinManager.SignOutAsync();
            return RedirectToAction("Index","Home");


        }

        public IActionResult Acces()
        {

            return View();
        }
    }
}