using MF.Common.Classes;
using MF.Common.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MF.Users.API.Controllers;


[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<MFUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserController(UserManager<MFUser> userManager, RoleManager<IdentityRole> roleManager)
	{
        _userManager = userManager;
        _roleManager = roleManager;
    }

    private async Task<IResult> AddUserAsync(string email, string password)
    {
        try
        {
            if (!ModelState.IsValid) return Results.BadRequest();

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser is not null) return Results.BadRequest();

            MFUser newUser = new()
            {
                Email = email,
                EmailConfirmed = true,
                UserName = email
            };

            IdentityResult result = await _userManager.CreateAsync(newUser, password);

            if (result.Succeeded) return Results.Ok();
        }
        catch { }

        return Results.BadRequest();
    }

    private async Task<IResult> AddRolesAsync(string email, List<string> roles)
    {
        try
        {
            if (!ModelState.IsValid) return Results.BadRequest();

            var user = await _userManager.FindByEmailAsync(email);
            if(user is null) return Results.BadRequest();

            var result = await _userManager.AddToRolesAsync(user, roles);
            if(result.Succeeded) return Results.Ok();
        }
        catch { }

        return Results.BadRequest();
    }

    [Route("api/users/seed")]
    [HttpPost]
    public async Task<IResult>Seed()
    {
        try
        {
            await _roleManager.CreateAsync(new IdentityRole { Id = "1", Name = UserRole.Admin });
            await _roleManager.CreateAsync(new IdentityRole { Id = "2", Name = UserRole.Customer });
            await _roleManager.CreateAsync(new IdentityRole { Id = "3", Name = UserRole.Registered });

            await AddUserAsync("Raspen@MF.com", "Pass123__");
            await AddUserAsync("Sylve@Bulk.com", "Pass123__");

            await AddRolesAsync("Raspen@MF.com", new List<string> { UserRole.Admin, UserRole.Registered, UserRole.Customer });
            await AddRolesAsync("Sylve@Bulk.com", new List<string> { UserRole.Registered, UserRole.Customer });

            return Results.Ok();
        }
        catch { }

        return Results.BadRequest();
    }

    [Route("api/user/register")]
    [HttpPost]
    public async Task<IResult> Register(RegisterUserDTO registerUserDTO)
    {
        try
        {
            var result = await AddUserAsync(registerUserDTO.Email, registerUserDTO.Password);
            if (result.Equals(Results.BadRequest())) return Results.BadRequest();
            result = await AddRolesAsync(registerUserDTO.Email, registerUserDTO.Roles);
            if (result.Equals(Results.BadRequest())) return Results.BadRequest();

            return Results.Ok();
        }
        catch { }

        return Results.BadRequest();
    }

    [Route("api/users/paid")]
    [HttpPost]
    public async Task<IResult> Paid(PaidCustomerDTO paidCustomerDTO)
    {
        return await AddRolesAsync(paidCustomerDTO.Email, new List<string> { UserRole.Customer });
    }
}
