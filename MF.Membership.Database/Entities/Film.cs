using System.Diagnostics.CodeAnalysis;

namespace MF.Membership.Database.Entities;

public class Film : IEntity
{
    public Film()
    {
        SimilarFilms = new HashSet<SimilarFilm>();
        Genres = new HashSet<Genre>();
    }
    public int Id { get; set; }
    [MaxLength(50)]
    public string? Title { get; set; }
    public DateTime Released { get; set; }
    [AllowNull]
    public int? DirectorId { get; set; } = null!;
    public bool Free { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; }
    [MaxLength(1024)]
    public string? FilmUrl { get; set; }
    [MaxLength(255)]
    public string? ThumbnailUrl { get; set; }

    public virtual Director? Director { get; set; }
    public virtual ICollection<Genre>? Genres { get; set; } = null!;
    public virtual ICollection<SimilarFilm>? SimilarFilms { get; set; } = null!;
}
