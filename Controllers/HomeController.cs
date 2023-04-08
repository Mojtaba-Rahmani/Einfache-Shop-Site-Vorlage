using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Controllers
{
    public class HomeController : Controller
    {
        IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index() => View();


        // baraye jahaye khas
        //[Authorize]
        //public IActionResult Test() => View();

        [Route("OnlinePayement/{id}")]
        public IActionResult OnlinePayement(int waletid)
        {
            if (HttpContext.Request.Query["Status"] !="" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                HttpContext.Request.Query["Authority"] != "")
            {
                var autorithy = HttpContext.Request.Query["Authority"];

                var Wallet = _userService.GetWalletByWalletId(waletid);

                var Peyment = new ZarinpalSandbox.Payment(Wallet.Amount);

                var res = Peyment.Verification(autorithy).Result;

                if (res.Status == 100)
                {
                    ViewBag.code = res.RefId;
                    ViewBag.IsSuccess = true;
                    Wallet.IsPay = true;
                    _userService.UpdateWallet(Wallet);
                }
            }
            return View();
        }

    }
}