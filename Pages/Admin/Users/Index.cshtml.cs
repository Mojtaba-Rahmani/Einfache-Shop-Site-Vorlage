using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Users
{
    public class IndexModel : PageModel
    {
        IUserService _IUserService;

        public IndexModel(IUserService IUserService)
        {
            _IUserService = IUserService;
        }
        // [Route("/Pages/Admin/Users")]
        public UserForAdminViewModel UserForAdminViewModel { get; set; }
        public void OnGet(int PageId = 1, string filterUserName = "", string filterEmail = "")
        {
            UserForAdminViewModel = _IUserService.GetUsers(PageId, filterUserName, filterEmail);
        }

        //public void OnGet()
        //{
        //    UserForAdminViewModel = _IUserService.GetUsers();
        //}
        //[HttpPost]
        //public IActionResult OnPost(int PageId = 1, string filterUserName = "", string filterEmail = "")
        //{
        //    UserForAdminViewModel = _IUserService.GetUsers(PageId, filterUserName, filterEmail);
        //    return Page();
        //}
    }
}
