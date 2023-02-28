
namespace MF.Token.API.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
    private readonly UserManager<MFUser> _userManager;

    public TokenService(IConfiguration configuration, IUserService userService, UserManager<MFUser> userManager)
	{
        _configuration = configuration;
        _userService = userService;
        _userManager = userManager;
    }
}
