using System.Reflection;

namespace MF.Common.Services;

public class MembershipService : IMembershipService
{
    private readonly MembershipHttpClient _http;

    public MembershipService(MembershipHttpClient http)
    {
        _http = http;
    }

    public async Task<List<FilmDTO>> GetFilmsAsync()
    {
        try
        {
            bool freeOnly = false;
            using HttpResponseMessage response = await _http.Client.GetAsync("Films");
            response.EnsureSuccessStatusCode();
            var result = JsonSerializer.Deserialize<List<FilmDTO>>(await
                response.Content.ReadAsStreamAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return result ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<FilmDTO> GetFilmAsync(int? id)
    {
        try
        {
            if (id is null)
                return new();

            using HttpResponseMessage response = await _http.Client.GetAsync($"Films/{id}");
            response.EnsureSuccessStatusCode();
            var result = JsonSerializer.Deserialize<FilmDTO>(await
                response.Content.ReadAsStreamAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return result ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<FilmSimilarDTO> GetFilmSimilarAsync(int? id)
    {
        try
        {
            if (id is null)
                return new();

            using HttpResponseMessage response = await _http.Client.GetAsync($"Films/{id}?isSimilar=true");
            response.EnsureSuccessStatusCode(); 
            var result = JsonSerializer.Deserialize<FilmSimilarDTO>(await
                response.Content.ReadAsStreamAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return result ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<List<GenreDTO>>GetGenresAsync()
    {
        try
        {
            using HttpResponseMessage response = await _http.Client.GetAsync("Genres");
            response.EnsureSuccessStatusCode();
            var result = JsonSerializer.Deserialize<List<GenreDTO>>(await
                response.Content.ReadAsStreamAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            return result ?? new();
        }
        catch
        {
            return new();
        }
    }
}
