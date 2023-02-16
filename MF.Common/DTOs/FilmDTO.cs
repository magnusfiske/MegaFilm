namespace MF.Common.DTOs;

public class FilmDirectorDTO
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public DateTime Released { get; set; }
    public int DirectorId { get; set; }
    public bool Free { get; set; }
    public string? Description { get; set; }
    public string? FilmUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
    public virtual List<string>? Genres { get; set; }
    public virtual List<string>? SimilarFilms { get; set; }
}

public class FilmDTO : FilmDirectorDTO
{
    public DirectorDTO? Director { get; set; }
}

public class FilmCreateDTO
{
    public string? Title { get; set; }
    public DateTime Released { get; set; }
    public int DirectorId { get; set; }
    public bool Free { get; set; }
    public string? Description { get; set; }
    public string? FilmUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
}


public class FilmEditDTO : FilmCreateDTO
{
    public int Id { get; set; }
}

