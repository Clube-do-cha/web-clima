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
                    UserName = model.FullName,
                    UserLogin = model.UserLogin,
                    Email = model.Email,
                    UserCity = model.UserCity
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Redirecionar para a página de perfil do usuário
                    return RedirectToAction("Profile", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
    }
}
