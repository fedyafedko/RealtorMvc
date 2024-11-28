using Microsoft.AspNetCore.Mvc;
using RealtorMVC.Models.Auth;
using RealtorMVC.BLL.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace RealtorMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IEmailConfirmationService _emailConfirmationService;

        public AuthController(IAuthService authService, IEmailConfirmationService emailConfirmationService)
        {
            _authService = authService;
            _emailConfirmationService = emailConfirmationService;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ConfirmEmail()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.SignInAsync(model);
                if (result != null)
                {
                    return Ok(result);
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _authService.SignUpAsync(model);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailModel model)
        {
            var result = await _emailConfirmationService.ConfirmEmailAsync(model);
            return Ok(result);
        }
    }
}
