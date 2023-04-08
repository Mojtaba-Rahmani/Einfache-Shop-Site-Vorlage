using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using TopLearn.Core.Convertors;
using TopLearn.Core.DTOs;
using TopLearn.Core.Generator;
using TopLearn.Core.Security;
using TopLearn.Core.Senders;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Controllers
{
    public class AccountController : Controller
    {
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }
        private IUserService _userService;
        private IViewRenderService _viewRenderService;

        public AccountController(IUserService userService, IViewRenderService viewRenderService)
        {
            _userService = userService;
            _viewRenderService = viewRenderService;
        }




        #region Register


        [Route("Register")]
        [HttpPost] // وقتی پست شد عمل میشه
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }
            if (_userService.IsExisUserName(register.UserName))
            {
                ModelState.AddModelError("UserName", "نام کاربری معتبر نمیباشد");
                return View(register);
            }
            if (_userService.IsExisEmail(FixedText.FixedEmail(register.Email)))
            {
                ModelState.AddModelError("Email", "ایمیل معتبر نمیباشد");
                return View(register);
            }

            //Todo : Register User

            DataLayer.Entities.User.User User = new DataLayer.Entities.User.User()
            {
                ActiveCode = NameGenerator.GeneratUniqCode(),
                Email = FixedText.FixedEmail(register.Email),
                IsActive = Convert.ToBoolean(false),
                Password = PasswordHelper.EncodePasswordMd5(register.Password),
                RegisterDate = DateTime.Now,
                UserAvatar = "/Par.jpeg",
                UserName = register.UserName,
                UserUniqId =NameGenerator.GeneratUniqUserId()//-------------------------
            };
            _userService.AddUser(User);




            #region Send Activation Email
            string body = _viewRenderService.RenderToStringAsync("_ActiveEmail", User);
            SendEmail.Send(User.Email, "فعال سازی", body);


            #endregion
            return View("SuccessRegister", User);
        }

        #endregion


        #region Login

        [Route("Login")]
        public ActionResult Login(bool EditProfile = false)
        {
            ViewBag.EditProfile = EditProfile;
            return View("Login");
        }

        [HttpPost]
        [Route("/Login")]
        public ActionResult Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            var user = _userService.LoginUser(login);
            if (user != null)
            {
                if (user.IsActive)
                {
                    // baraye ehraze hovyat va login
                    var Claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim("UserUniqId",user.UserUniqId),
                    };
                    var IdentityUser = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var Principal = new ClaimsPrincipal(IdentityUser);

                    var Properties = new AuthenticationProperties
                    {
                        IsPersistent = login.RemmemberMe

                    };
                    HttpContext.SignInAsync(Principal, Properties);

                    //
                    ViewBag.Issuccess = true;
                    return View();
                }


            }
            else
            {
                ModelState.AddModelError("Email", "حساب کاربری شما فعال نمیباشد");
            }
            ModelState.AddModelError("Email", "کاربری با این مشخصات یافت نشد");
            return View(login);
        }

        #endregion

        #region Active Accouant

        //[Route("ActiveAccount")]
        public ActionResult ActiveAccount(string Id)
        {
            ViewBag.IsActive = _userService.ActiveAccouant(Id);
            return View();
        }



        #endregion

        #region Logaout
        [Route("Logaout")]
        public ActionResult Logaout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }
        #endregion


        #region Forgot Password
        [Route("ForgotPassword")]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public ActionResult ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            if (!ModelState.IsValid)
                return View(forgotPassword);

            string FixedEmail = FixedText.FixedEmail(forgotPassword.Email);
            DataLayer.Entities.User.User user = _userService.GetUserByEmail(FixedEmail);

            if (user == null)
            {
                ModelState.AddModelError("Email", "کاربری یافت نشد");
                return View(forgotPassword);
            }
            string bodyEmail = _viewRenderService.RenderToStringAsync("_ForgotPassword", user);
            SendEmail.Send(user.Email, "بازیابی حساب کاربری", bodyEmail);
            ViewBag.IsSuccess = true;
            return View();
        }
        #endregion

        #region Reser Password
      
        public ActionResult ResetPassword(string id)
        {

           // DataLayer.Entities.User.User user = PasswordHelper.EncodePasswordMd5(user.Email);
            return View(new ResetPasswordViewModel()
            {
                ActiveCode = id
                
            }) ;
        }

        [HttpPost] //metode bazgashtye Action
        [Route("ResetPassword")]
        public ActionResult ResetPassword(ResetPasswordViewModel resetPassword)
        {
            if (!ModelState.IsValid)
                return View(resetPassword);
            DataLayer.Entities.User.User user = _userService.GetUserByActiveCode(resetPassword.ActiveCode);
            if (user == null)
                return NotFound();

         string HachNewPass = PasswordHelper.EncodePasswordMd5(resetPassword.Password);
            user.Password = HachNewPass;
            _userService.UpdateUser(user);

            return Redirect("/Login");
        }
        #endregion
    }
}
