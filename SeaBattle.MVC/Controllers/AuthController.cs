using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Enums;
using SeaBattle.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeaBattle.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Name, model.Password, true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterModel model)
        {


            if (ModelState.IsValid)
            {
                var user = new User(model.Email, model.LoginModel.Password, RoleType.CommonUser, model.LoginModel.Name);

                var result = await _userManager.CreateAsync(user, user.Password);

                if (result.Succeeded)
                {
                    await _signInManager.PasswordSignInAsync(model.LoginModel.Name, model.LoginModel.Password, true, lockoutOnFailure: false);

                    return RedirectToAction("Index", "Home");
                }

                return View(model);
            }

            return View(model);
        }

    }
}
