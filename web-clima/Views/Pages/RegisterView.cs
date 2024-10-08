﻿using System.ComponentModel.DataAnnotations;

namespace web_clima.Views.Pages
{
    public class RegisterView
    {
        [Required]
        [Display(Name = "Nome Completo")]
        public string FullName { get; set; }  // Renomeado para corresponder ao modelo UserModel

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [Compare("Password", ErrorMessage = "A senha e a confirmação da senha não coincidem.")]
        public string ConfirmPassword { get; set; }
    }
}
