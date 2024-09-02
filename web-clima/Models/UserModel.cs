using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace web_clima.Models
{
    public class UserModel : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [StringLength(50)]
        public string UserLogin { get; set; }

        [StringLength(100)]
        public string UserCity { get; set; }

        public byte[] UserProfilePic { get; set; }
    }
}
