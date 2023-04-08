using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.DTOs;
using TopLearn.Core.Services;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Areas.UserPanel.Controllers
{
  
        [Area("UserPanel")]
        [Authorize]
        public class WalletController : Controller
        {
            private IUserService _userService;

            public WalletController(IUserService userService)
            {
                _userService = userService;
            }



            [Route("UserPanel/Wallet")]
            public IActionResult Index()
        {
            ViewBag.ListWallet = _userService.GetWalletUser(User.Identity.Name);
            return View();
        }


        [Route("UserPanel/Wallet")]
        [HttpPost]
        public IActionResult Index(ChargeWalletViewModel ChargWallet)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ListWallet = _userService.GetWalletUser(User.Identity.Name);
                return View(ChargWallet);
            }

            int WalletId = _userService.ChargeWallet(User.Identity.Name, ChargWallet.Amount, "شارج  حساب");


            #region Online Peyment

            var Peyment = new ZarinpalSandbox.Payment(ChargWallet.Amount);

            var res = Peyment.PaymentRequest("شارج کیف پول", "https://localhost:44349/OnlinePayement/" + WalletId+"mojiixx67@gmail.com","09120399737");

            if (res.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);
            }

            #endregion
            return null;
        }

    }
}
