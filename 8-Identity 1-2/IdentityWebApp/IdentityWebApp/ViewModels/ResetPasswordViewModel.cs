﻿using System.ComponentModel.DataAnnotations;

namespace IdentityWebApp.ViewModels
{
    public class ResetPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        [Display(Name = "Yeni şifre :")]
        public string Password { get; set; }




        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Şifre aynı değildir.")]
        [Required(ErrorMessage = "Şifre tekrar alanı boş bırakılamaz")]
        [Display(Name = "Yeni şifre Tekrar :")]
        public string PasswordConfirm { get; set; }
    }
}
