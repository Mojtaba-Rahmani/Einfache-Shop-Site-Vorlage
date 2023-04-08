using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TopLearn.Core.DTOs;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")] // moshakhas mikone ke in Home motealegh be Area ast vaa dar in bala public tarif mishavad
    [Authorize]

    public class HomeController : Controller
    {

        public string _UserUniq;  // the name field
        public string UserUniqId    // the Name property
        {
            get => User.FindFirst("UserUniqId").Value.ToString();
            set => _UserUniq = UserUniqId;
        }

        //public string _UserUniqId = User.FindFirst("UserUniqId").Value.ToString();
        IUserService _UserService;
        public HomeController(IUserService userService)
        {
            _UserService = userService;
        }

        public IActionResult Index()
        {
            return View(_UserService.GetUserInformation(Convert.ToInt32(UserUniqId)));
        }

        //
        [Route("/UserPanel/EditProfile")]
        public IActionResult EditProfile()
        {
            return View(_UserService.GetDataForEditProfileUser(UserUniqId));
        }

        [Route("/UserPanel/EditProfile")]
        [HttpPost]
        public IActionResult EditProfile(EditProfileVieModel Profile)
        {
            if (!ModelState.IsValid)
            //return View(Profile);
            {

                ViewBag.Issuccess = true;
                _UserService.EditProfile(Convert.ToInt32(UserUniqId), Profile);
                //HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                //return Redirect("/Login?EditProfile=True");
                return View(_UserService.GetDataForEditProfileUser(UserUniqId));
            }
            else
            {
                ViewBag.NotSuccess = true;
                return View(Profile);
            }
        }


        [Route("/UserPanel/ChangePassword")]
        public IActionResult ChangePassword()
        {

            return View();
        }

        [Route("/UserPanel/ChangePassword")]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel change)
        {
            string CurrentUser = User.Identity.Name;
            if (!ModelState.IsValid)
                return View(change);
            else
            {


                if (!_UserService.CompareOldPassword(change.OldPassword, CurrentUser))
                {
                    ModelState.AddModelError("OldPassword", "کلمه عبور فعلی صحیح نمیباشد");
                    ViewBag.NotSuccess = true;
                    return View(change);
                }
                else
                {
                    _UserService.ChangeUserPassword(CurrentUser, change.Password);
                    ViewBag.IsSuccess = true;
                    return View(change);
                }

            }
          

        }


       
    }
}
