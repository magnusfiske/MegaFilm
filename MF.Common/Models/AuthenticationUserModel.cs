
namespace MF.Common.Models;

public class AuthenticationUserModel
{
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}
