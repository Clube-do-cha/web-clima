using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using web_clima.Models;
using System.Threading.Tasks;

public class RegisterModel : PageModel
{
    private readonly UserManager<UserModel> _userManager;
    private readonly SignInManager<UserModel> _signInManager;

    public RegisterModel(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [BindProperty]
    public RegisterView Input { get; set; }

    public void OnGet()
    {
        // Prepare any data needed for the page (if any)
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = new UserModel
        {
            UserName = Input.FullName, // Corrigido para FullName
            Email = Input.Email,
            // Adicionar outros campos necessários
            FullName = Input.FullName, // Adicionado para corresponder ao modelo UserModel
            UserLogin = Input.UserLogin,
            UserCity = Input.UserCity
        };

        var result = await _userManager.CreateAsync(user, Input.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToPage("/Index"); // Redirecionar após o registro
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return Page();
    }
}
