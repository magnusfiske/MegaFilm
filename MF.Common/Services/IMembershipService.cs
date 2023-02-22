namespace MF.Common.Services
{
    public interface IMembershipService
    {
        Task<FilmDTO> GetFilmAsync(int? id);
        Task<List<FilmDTO>> GetFilmsAsync();
        Task<FilmSimilarDTO> GetFilmSimilarAsync(int? id);
        Task<List<GenreDTO>> GetGenresAsync();
    }
}