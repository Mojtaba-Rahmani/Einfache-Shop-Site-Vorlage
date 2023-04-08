using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using TopLearn.Core.DTOs;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Users
{
    public class EditUserModel : PageModel
    {
        private IUserService _userService;
        private IPermissionService _permissionService;
        public EditUserModel(IUserService userService, IPermissionService permissionServic)
        {
            _userService = userService;
            _permissionService = permissionServic;
        }
        [BindProperty]
        public UserEditViewModel UserEditViewModel { get; set; }
        public void OnGet(int id)
        {
            UserEditViewModel = _userService.GetUserForShowInEditMode(id);
            //ViewData["Roles"] = _userService.GetUserForShowInEditMode(id).UserRoles;
            ViewData["Roles"] = _permissionService.GetRoles();
        }

        public IActionResult OnPost(List<int> SeletRolls)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _userService.EditUserFromAdmin(UserEditViewModel);
            // Edit Rolles

            _permissionService.EditRolleUser(SeletRolls, UserEditViewModel.UserId);
            return RedirectToPage("Index");
        }

    }
}
