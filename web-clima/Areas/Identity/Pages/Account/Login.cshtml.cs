using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using web_clima.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

public class LoginModel : PageModel
{
    private readonly SignInManager<UserModel> _signInManager;
    private readonly ILogger<LoginModel> _logger;
    private readonly UserManager<UserModel> _userManager;

    public LoginModel(
        SignInManager<UserModel> signInManager,
        ILogger<LoginModel> logger,
        UserManager<UserModel> userManager)
    {
        _signInManager = signInManager;
        _logger = logger;
        _userManager = userManager;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }

    public class InputModel
    {
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }

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
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            _logger.LogInformation("Attempting login for user: {Login}", Input.Login);

            // Find the user by login
            var user = await _userManager.FindByNameAsync(Input.Login);

            if (user != null)
            {
                // Retrieve the stored password hash
                var storedHash = user.PasswordHash;

                // Manually verify the provided password with the stored hash
                if (VerifyPasswordHash(Input.Password, storedHash))
                {
                    // Sign in the user
                    await _signInManager.SignInAsync(user, Input.RememberMe);

                    _logger.LogInformation("User logged in: {Login}", Input.Login);
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    _logger.LogWarning("Invalid login attempt: {Login}", Input.Login);
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            else
            {
                _logger.LogWarning("User not found: {Login}", Input.Login);
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
        }
        else
        {
            _logger.LogWarning("Model state is invalid.");
        }

        // If we got this far, something failed, redisplay the form
        return Page();
    }

    private bool VerifyPasswordHash(string password, string storedHash)
    {
        try
        {
            // Instantiate PasswordHasher
            var hasher = new PasswordHasher<UserModel>();

            // Verify the provided password against the stored hash
            var result = hasher.VerifyHashedPassword(null, storedHash, password);

            return result == PasswordVerificationResult.Success;
        }
        catch
        {
            _logger.LogError("Error verifying password hash.");
            return false;
        }
    }
}
