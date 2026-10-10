using System.ComponentModel.DataAnnotations;

namespace Comasy.Web.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Ange e-postadress")]
    [EmailAddress(ErrorMessage = "Ogiltig e-postadress")]
    [Display(Name = "E-post")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ange lösenord!")]
    [DataType(DataType.Password)]
    [Display(Name = "Lösenord")]
    public string Password { get; set; } = string.Empty;
    
    [Display(Name = "Kom ihåg mig")]
    public bool RememberMe { get; set; }
    
    public string? ReturnUrl { get; set; }

}