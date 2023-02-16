namespace MF.Common.DTOs; 
public class GenreDTO 
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public virtual List<FilmDTO>? Films { get; set; }
}
