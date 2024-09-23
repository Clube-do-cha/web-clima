using System.ComponentModel.DataAnnotations; // Importante para as anotações de validação
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using web_clima.Models;

namespace web_clima.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;

        public IndexModel(
            UserManager<UserModel> userManager,
            SignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string FullName { get; set; }
        public string Email { get; set; } // Adicionando a propriedade para o e-mail

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Não foi possivel registrar esse nome de usuario")]
            [Display(Name = "Nome")]
            public string FullName { get; set; }

            [Phone(ErrorMessage = "O número de telefone deve ser um número válido.")]
            [Display(Name = "Número de telefone")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(UserModel user)
        {
            var fullName = user.FullName; // Presumindo que você tenha uma propriedade FullName no UserModel
            var email = await _userManager.GetEmailAsync(user); // Carregando o e-mail
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            FullName = fullName;
            Email = email; // Armazenando o e-mail

            Input = new InputModel
            {
                FullName = fullName,
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // Atualizar o nome completo
            if (Input.FullName != FullName)
            {
                user.FullName = Input.FullName; // Presumindo que você tenha uma propriedade FullName no UserModel
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    StatusMessage = "Erro inesperado ao tentar mudar o nome completo.";
                    return RedirectToPage();
                }
            }

            // Atualizar o número de telefone
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Erro inesperado ao tentar definir o número de telefone.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Seu perfil foi atualizado";
            return RedirectToPage();
        }
    }
}