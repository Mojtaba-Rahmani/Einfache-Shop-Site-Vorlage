﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.DTOs
{
    public class UserForAdminViewModel
    {
        public List<User> Users { get; set; }
        public int CurrentPage { get; set; }

        public int PageCount { get; set; }
    }
    public class SaveAvatarViewModel
    {
        public int UserId { get; set; }
        public IFormFile UserAvatar { get; set; }
    }
    public class CreateUsersViewModel
    {

        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Password { get; set; }

        public IFormFile UserAvatar { get; set; }

        public List<int> SeletRolls { get; set; }

    }

}
