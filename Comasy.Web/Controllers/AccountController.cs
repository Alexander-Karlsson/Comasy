using Comasy.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Comasy.Web.Controllers;

// SignInManager sköter inloggning, utloggning, cookie och lockout
// (PasswordSignInAsync / SignOutAsync). IdentityUser är Identitys
// standardanvändare — jag behövde inga extra fält, så ingen egen user-klass.
public class AccountController(SignInManager<IdentityUser> signInManager) : Controller
{
    // Visar inloggningsformuläret. ReturnUrl används efter lyckad inloggning.
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    // Tar emot formuläret, loggar in med e-post/lösenord och hanterar lockout.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction("Index", "Page", new { slug = "valkommen" });
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(string.Empty,
                "Kontot är tillfälligt låst efter för många felförsök.");
            
            return View(model);
        }
        
        ModelState.AddModelError(string.Empty, "Fel e-postadress eller lösenord!");
        return View(model);
    }

    // Loggar ut den inloggade användaren och skickar tillbaka till startsidan.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index" , "Page", new { slug = "valkommen" });
    }

    // Visas när en inloggad användare saknar behörighet till en sida.
    [HttpGet]
    public IActionResult Denied() => View();
}