using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using web_clima.Models;
using web_clima.Views.Pages;

namespace web_clima.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;

        public RegisterController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View("/Views/Home/Index.cshtml");;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterView model)
        {
            if (!ModelState.IsValid) return View("/Views/Home/Index.cshtml");;
            var user = new UserModel
            {
                FullName = model.FullName,
                Email = model.Email,
                IsAdmin = false
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Sinaliza o usuário e redireciona para a página de perfil
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Profile", "Account"); // Certifique-se de que o método Profile existe em AccountController
            }

            // Adiciona os erros ao modelo
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // Retorna a view com o modelo caso haja falhas de validação
            return View("/Views/Home/Index.cshtml");
        }
    }
}
