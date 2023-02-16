

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using MF.Membership.Database.Services;

namespace MF.Membership.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IDbService _db;

    public GenresController(IDbService db)
    {
        _db = db;
    }
    // GET: api/<GenresController>
    [HttpGet]
    public async Task<IResult> Get()
    {
        try
        {
            _db.Include<Genre>();
            _db.IncludeRef<FilmGenre>();
            var genres = await _db.GetAsync<Genre, GenreDTO>();
            return Results.Ok(genres);
        }
        catch { }

        return Results.NotFound();
    }

    // GET api/<GenresController>/5
    [HttpGet("{id}")]
    public async Task<IResult> Get(int id)
    {
        try
        {
            _db.Include<Genre>();
            var genre = await _db.SingleAsync<Genre, GenreDTO>(i => i.Id == id);
            return Results.Ok(genre);
        }
        catch { }

        return Results.NotFound();
    }

    // POST api/<GenresController>
    [HttpPost]
    public async Task<IResult> Post(GenreCreateDTO dto)
    {
        try
        {
            if (dto == null) { return Results.BadRequest(); }

            var entity = await _db.AddAsync<Genre, GenreCreateDTO>(dto);

            if (await _db.SaveChangesAsync())
            {
                var node = typeof(Genre).Name.ToLower();
                return Results.Created($"/{node}s/{entity.Id}", dto);
            }

        }
        catch { }

        return Results.BadRequest();
    }

    // PUT api/<GenresController>/5
    [HttpPut("{id}")]
    public async Task<IResult> Put(int id, GenreEditDTO dto)
    {
        try
        {
            if (dto == null || id != dto.Id) { return Results.BadRequest(); }

            if (!await _db.AnyAsync<Genre>(i => i.Id.Equals(id)))
                return Results.NotFound();

            _db.Update<Genre, GenreEditDTO>(id, dto);

            var success = await _db.SaveChangesAsync();

            if (!success)
                return Results.BadRequest();

            return Results.NoContent();
        }
        catch { }
        return Results.NotFound();
    }

    // DELETE api/<GenresController>/5
    [HttpDelete("{id}")]
    public async Task<IResult> Delete(int id)
    {
        try
        {
            var success = await _db.DeleteAsync<Genre>(id);
            if (!success) return Results.NotFound();

            success = await _db.SaveChangesAsync();
            if (!success) return Results.BadRequest();

            return Results.NoContent();

        }
        catch { }

        return Results.BadRequest();
    }
}
