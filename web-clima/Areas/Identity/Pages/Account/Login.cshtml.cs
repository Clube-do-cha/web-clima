using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using web_clima.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

public class LoginModel(
    SignInManager<UserModel> signInManager,
    ILogger<LoginModel> logger,
    UserManager<UserModel> userManager)
    : PageModel
{
    private readonly UserManager<UserModel> _userManager = userManager;

    [BindProperty]
    public required InputModel Input { get; set; }

    public required string ReturnUrl { get; set; }

    [TempData]
    public required string ErrorMessage { get; set; }

    public class InputModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(string returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl ??= Url.Content("~/");

        // Sign out any existing external cookies
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ReturnUrl = returnUrl;
        return Page();
    }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            // Define o redirecionamento padrão para o Dashboard
            returnUrl ??= Url.Content("~/Dashboard/Index");

        if (ModelState.IsValid)
        {
            logger.LogInformation("Tentando logar usuário com e-mail: {Email}", Input.Email);

            // Find the user by e-mail
            var result = await signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                logger.LogInformation("User logged in: {Email}", Input.Email);
                return LocalRedirect(returnUrl);
            }
        }
        
        logger.LogWarning("Invalid login attempt for: {Email}", Input.Email);
        ModelState.AddModelError(string.Empty, "Tentativa de login inválida.");

        return Page();
    }
}
