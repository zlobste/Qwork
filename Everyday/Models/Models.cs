using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Everyday.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "ФИО")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Повторить пароль")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Дата рождения")]
        [Required]
        public DateTime UserAge { get; set; }


        [RegularExpression (@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        [Required]
        [Display(Name = "Email")]
        public string UserEmail { get; set; }


        [Required]
        [Display(Name = "Мобильный")]
        public string UserMobileNumber { get; set; }



    }
}