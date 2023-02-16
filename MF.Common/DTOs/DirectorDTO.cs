namespace MF.Common.DTOs;

public class DirectorCreateDTO
{
    public string? Name { get; set; }

}

public class DirectorDTO : DirectorCreateDTO
{
    public int Id { get; set; }
    public List<FilmDirectorDTO>? Films { get; set; }
}
