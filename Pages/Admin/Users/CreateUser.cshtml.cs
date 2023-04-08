using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using TopLearn.Core.DTOs;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Users
{
    public class CreateUserModel : PageModel
    {
        private IUserService _userService;
        private IPermissionService _permissionService;
        public CreateUserModel(IUserService userService, IPermissionService permissionService)

        {
            _userService = userService;
            _permissionService = permissionService;
        }
        [BindProperty]
        public CreateUsersViewModel CreateUserViewModel { get; set; }
        public void OnGet()
        {
            ViewData["Roles"] = _permissionService.GetRoles();
        }

        public IActionResult OnPost(List<int> SeletRolls)
        {
            if(!ModelState.IsValid)
            return Page();

            int UserId = _userService.AddUserFromAdmin(CreateUserViewModel);

            // Add Roles

            _permissionService.AddRollesToUser(SeletRolls, UserId);

            return Redirect("/Admin/Users");
        }
    }
}
