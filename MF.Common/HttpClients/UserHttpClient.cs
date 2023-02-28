using MF.Common.Classes;
using MF.Common.Models;
using System.Text;

namespace MF.Common.HttpClients;

public class UserHttpClient
{
    public UserHttpClient(HttpClient client)
	{
        Client = client;
    }

    public HttpClient Client { get; }  

    public async Task CreateUser(CreateUserModel model)
    {
        try
        {
            if (model == null) throw new ArgumentException("CreateUserModel is null");

            List<string> roles = new List<string>() { UserRole.Registered };
            if(model.IsCustomer == true) roles.Add(UserRole.Customer);
            if(model.IsAdmin == true) roles.Add(UserRole.Admin);

            RegisterUserDTO user = new RegisterUserDTO(model.Email, model.Password, roles);

            using StringContent jsonContent = new(
                JsonSerializer.Serialize(user),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await Client.PostAsync("users/register", jsonContent);
            if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);
        }
        catch 
        {
            throw;
        }
    }

    public async Task PayingCustomer(PaidCustomerDTO model)
    {
        try
        {
            if (model == null) throw new ArgumentException("PaidCustomerDTO is null");

            using StringContent jsonContent = new(
                JsonSerializer.Serialize(model),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await Client.PostAsync("users/paid", jsonContent);
            if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);
        }
        catch
        {
            throw;
        }
    }
}
