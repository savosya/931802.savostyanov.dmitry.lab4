using Microsoft.AspNetCore.Mvc;
using lab4.Models;
using lab4.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using lab4.ViewModel;

namespace lab4.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserContext _userDB;
        public AccountController(UserContext context)
        {
            _userDB = context;
        }

        [HttpGet]
        public IActionResult SignUp_1()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignedIn");
            }
            return View();
        }
        [HttpPost]
        public IActionResult SignUp_1(RegisterModel1 model)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetObjectAsJson("halfreg", model);
                return RedirectToAction("SignUp_2");
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult SignUp_2()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp_2(RegisterModel2 model)
        {
            if (ModelState.IsValid)
            {
                RegisterModel1 halfmodel = HttpContext.Session.GetObjectFromJson<RegisterModel1>("halfreg");
                UserModel user = new();

                _userDB.Users.Add(new UserModel
                {
                    FirstName = halfmodel.FirstName,
                    LastName = halfmodel.LastName,
                    Birthday = halfmodel.Birthday,
                    Gender = halfmodel.Gender,
                    Email = model.Email,
                    Password = model.Password,
                    ToRemember = model.ToRemember
                });
                

                await _userDB.SaveChangesAsync();
                return RedirectToAction("LogIn");
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult LogIn()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignedIn");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel user = await _userDB.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(model.Email); // аутентификация

                    return RedirectToAction("SignedIn");
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Некорректные логин и(или) пароль");
                    return View();
                }

            }
            return View(model);
        }


        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("MultStagedForms", "Home");
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SignedIn()
        {
            string email = HttpContext.User.Identity.Name;
            UserModel user = await _userDB.Users.FirstOrDefaultAsync(u => u.Email == email);

            return View(user);
        }


        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }


        [HttpGet]
        public IActionResult ResetPw_1()
        { return View(); }
        [HttpPost]
        public async Task<IActionResult> ResetPw_1(LoginModel model, string action)
        {
            if (action == "Send me a code")
            {
                if (model.Email != null)
                {
                    var user = await _userDB.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError("Email", "Email не найден.");
                    }
                    else
                    {
                        string code = "";
                        Random rnd = new Random();
                        for (int i = 0; i < 4; i++)
                        {
                            code += rnd.Next(0, 10);
                        }
                        ViewBag.code = code;
                        HttpContext.Session.SetString("ConformationCode", code);
                        return RedirectToAction("ResetPw_2");
                    }
                }
                ModelState.AddModelError("Email", "Введите email.");
                return View();

            }
            HttpContext.Session.SetString("ConformationCode", "1234");
            return RedirectToAction("ResetPw_2");
        }


        [HttpGet]
        public IActionResult ResetPw_2()
        {
            ViewBag.code = HttpContext.Session.GetString("ConformationCode");
            return View();
        }
        [HttpPost]
        public IActionResult ResetPw_2(TextInputModel model)
        {
            string ConformationCode = HttpContext.Session.GetString("ConformationCode");
            if (model.Text != null)
            {
                if (model.Text == ConformationCode)
                {
                    return RedirectToAction("ResetPw_3");
                }
                ModelState.AddModelError("Text", "Неверный код.");

            }
            ModelState.AddModelError("Text", "Введите код.");
            return View();
        }


        [HttpGet]
        public IActionResult ResetPw_3()
        { return View(); }
        [HttpPost]
        public async Task<IActionResult> ResetPw_3(ConfirmPwModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.Password == model.ConfirmPassword)
                {
                    string email = HttpContext.User.Identity.Name;
                    UserModel user = await _userDB.Users.FirstOrDefaultAsync(u => u.Email == email);
                    if(user != null)
                    {
                        user.Password = model.Password;
                    }                    
                    await _userDB.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError("ConfirmPassword", "Пароли не совпадают.");
                    return View(model);
                }
            } else
            {
                return View(model);
            }
            return RedirectToAction("SignedIn");
        }

    }

}
