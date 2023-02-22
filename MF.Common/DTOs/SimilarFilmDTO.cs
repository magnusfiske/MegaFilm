
namespace MF.Common.DTOs;

public class SimilarFilmDTO
{
    public int ParentFilmId { get; set; }
    public int SimilarFilmId { get; set; }

    //public virtual FilmDTO? Film { get; set; }
    public virtual FilmEditDTO? Similar { get; set; }
}

public class SimilarFilmCreateDTO
{
    public int ParentFilmId { get; set; }
    public int SimilarFilmId { get; set; }
}
