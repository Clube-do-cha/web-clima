using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace web_clima.Models
{
    public class UserModel : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = "Default Name"; // Valor padrão

        [DefaultValue(false)]
        public bool IsAdmin { get; set; } = false;

        public byte[]? UserProfilePic { get; set; } // Tornar a propriedade nula se for opcional
    }
}