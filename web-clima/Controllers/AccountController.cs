using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using web_clima.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace web_clima.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<UserModel> _userManager;

        public AccountController(UserManager<UserModel> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user); // Retorna a visualização Profile.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> DisableLockout(string username)
        {
            // Encontrar o usuário pelo nome de usuário
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                // Desativar o lockout para o usuário
                user.LockoutEnabled = false;
                await _userManager.UpdateAsync(user);

                // Adicione um log ou mensagem de sucesso se desejar
                TempData["Message"] = "Lockout desativado para o usuário.";
            }
            else
            {
                // Usuário não encontrado
                TempData["Error"] = "Usuário não encontrado.";
            }

            return RedirectToAction("Profile"); // Redireciona para a página de perfil ou outra página apropriada
        }
    }
}
