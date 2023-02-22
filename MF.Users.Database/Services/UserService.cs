namespace MF.Users.Database.Services;

public class UserService : IUserService
{
    private readonly UserManager<MFUser> _userManager;

    public UserService(UserManager<MFUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<MFUser> GetUserAsync(string email)
    {
        try
        {
            return await _userManager.FindByEmailAsync(email);
        }
        catch
        {
        }

        return default;
    }

    public async Task<MFUser> GetUserAsync(LoginUserDTO loginUser)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginUser.Email);
            if (user == null) return default;

            var hasher = new PasswordHasher<MFUser>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, loginUser.Password);

            if (result.Equals(PasswordVerificationResult.Success)) return user;
        }
        catch
        {
        }

        return default;
    }
}
