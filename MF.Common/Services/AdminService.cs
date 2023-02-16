using System.Net.Http.Json;
using System.Text;

namespace MF.Common.Services;

public class AdminService : IAdminService
{
    private readonly MembershipHttpClient _http;

    public AdminService(MembershipHttpClient http)
    {
        _http = http;
    }

    public async Task<List<TDto>> GetAsync<TDto>(string uri)
    {
        try
        {
            using HttpResponseMessage response = await _http.Client.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<List<TDto>>(await
                response.Content.ReadAsStreamAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            return result ?? new List<TDto>(); //?? kollar om null
            //if (result == null)
            //    return new List<TDto>();

            //return result;
        }
        catch
        {
            throw;
        }
    }

    public async Task<TDto> SingleAsync<TDto>(string uri)
    {
        try
        {
            using HttpResponseMessage response = await _http.Client.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<TDto>(await
                response.Content.ReadAsStreamAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });


            return result ?? default;
            //if (result == null)
            //    return default;

            //return result;
        }
        catch
        {
            throw;
        }
    }

    public async Task CreateAsync<TDto>(string uri, TDto dto)
    {
        try
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(dto),
                Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await _http.Client.PostAsync(uri, jsonContent);
            response.EnsureSuccessStatusCode();
        }
        catch
        {
            throw;
        }
    }

    public async Task EditAsync<TDto>(string uri, TDto dto)
    {
        try
        {
            //Alternativ metod:
            //await _http.Client.PutAsJsonAsync(uri, dto);

            using StringContent jsonContent = new(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8, "application/json");

            using HttpResponseMessage respons = await _http.Client.PutAsync(uri, jsonContent);
            respons.EnsureSuccessStatusCode();
        }
        catch
        {
            throw;
        }
    }

    public async Task DeleteAsync<TDto>(string uri)
    {
        try
        {
            using HttpResponseMessage respons = await _http.Client.DeleteAsync(uri);
            respons.EnsureSuccessStatusCode();
        }
        catch
        {
            throw;
        }
    }
}
