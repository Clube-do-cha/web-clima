using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace web_clima.Models
{
    public class UserModel : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        public string UserLogin { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        [StringLength(100)]
        public string UserCity { get; set; }

        public byte[] UserProfilePic { get; set; }
    }
}
