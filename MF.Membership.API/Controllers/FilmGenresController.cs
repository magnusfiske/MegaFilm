using MF.Membership.Database.Contexts;
using MF.Membership.Database.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MF.Membership.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilmGenresController : ControllerBase
{
    private readonly IDbService _db;

    public FilmGenresController(IDbService db)
    {
        _db = db;
    }
    // GET: api/<FilmGenresController>
    [HttpGet]
    public async Task<IResult> Get() =>
        Results.Ok(await _db.GetRefAsync<FilmGenre, FilmGenreDTO>());

    //GET api/<FilmGenresController>/5
    [HttpGet("{genreId:int}", Name ="Get")]
    public async Task<IResult> Get(int genreId)
    {
        try
        {
            var films = await _db.GetFilmsInGenreAsync(genreId);
            return Results.Ok(films);
        }
        catch
        {
            return Results.BadRequest();
        }
        return Results.NotFound();
    }

    // POST api/<FilmGenresController>
    [HttpPost]
    public async Task<IResult> Post(FilmGenreDTO dto)
    {
        try
        {
            var entity = await _db.AddRefAsync<FilmGenre, FilmGenreDTO>(dto);
            if (await _db.SaveChangesAsync())
            {
                return Results.Ok();
            }
        }
        catch (Exception ex)
        {
            return Results.BadRequest($"Couldn't add the {typeof(FilmGenre).Name}" +
            $"entity.\n{ex}.");
        }
        return Results.BadRequest($"Couldn't add the {typeof(FilmGenre).Name}" +
            $"entity.");
    }

    // PUT api/<FilmGenresController>/5
    [HttpPut("{id}")]
    public async Task<IResult> Put(int filmId, FilmGenreDTO dto)
    {
        try
        {
            if (dto == null || filmId != dto.FilmId)
                return Results.BadRequest();
            if (!await _db.AnyRefAsync<FilmGenre>(e => e.FilmId.Equals(filmId)))
                return Results.NotFound();
            _db.UpdateFilmGenre(filmId, dto);
            var success = await _db.SaveChangesAsync();
            return success ? Results.NoContent() : Results.BadRequest();
        }
        catch
        {
            throw;
        }
    }

    // DELETE api/<FilmGenresController>/5
    [HttpDelete("{filmId:int}/{genreId:int}", Name = "Delete")]
    public async Task<IResult> Delete(int filmId, int genreId)
    {
        try
        {
            //_db.IncludeRef<FilmGenre>();
            var dto = await _db.SingleRefAsync<FilmGenre, FilmGenreDTO>(f => (f.FilmId.Equals(filmId)) && (f.GenreId.Equals(genreId)));
            

            var success = await _db.DeleteRefAsync<FilmGenre, FilmGenreDTO>(dto, filmId, genreId);

            if (!success) return Results.BadRequest();

            success = await _db.SaveChangesAsync();

            if (!success) return Results.BadRequest();
            //var contextOptions = new DbContextOptionsBuilder<MFContext>()
            //    .UseSqlServer(@"Server=(localdb)\\mssqllocaldb;Database=MFMembershipDb")
            //    .Options;
            //using (var db = new MFContext(contextOptions)) 
            //{

            //    db.Remove(entity);
            //    var success = await db.SaveChangesAsync();
            //    if (success < 0) return Results.BadRequest();
            //}

            return Results.NoContent();
        }
        catch
        {
            throw;
        }
    }
}
