using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using web_clima.Models;
using System.Threading.Tasks;

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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterView model)
        {
            if (ModelState.IsValid)
            {
                var user = new UserModel
                {
                    UserName = model.UserLogin, // UserName deve ser igual ao UserLogin
                    FullName = model.FullName,
                    UserLogin = model.UserLogin,
                    Email = model.Email,
                    UserCity = model.UserCity
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
            }

            // Retorna a view com o modelo caso haja falhas de validação
            return View(model);
        }
    }
}
