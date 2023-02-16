namespace MF.Membership.Database.Entities;

public class FilmGenre : IReferenceEntity
{
    public int FilmId { get; set; }
    public int GenreId { get; set; }

    public virtual Film Film { get; set; } = null!;
    public virtual Genre Genre { get; set; } = null!;
}
