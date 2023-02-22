using MF.Common.DTOs;

namespace MF.Users.Database.Services
{
    public interface IUserService
    {
        Task<MFUser> GetUserAsync(LoginUserDTO loginUser);
        Task<MFUser> GetUserAsync(string email);
    }
}