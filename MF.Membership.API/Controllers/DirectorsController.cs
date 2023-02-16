using MF.Membership.Database.Entities;
using MF.Membership.Database.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MF.Membership.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DirectorsController : ControllerBase
{
    private readonly IDbService _db;

    public DirectorsController(IDbService db)
    {
        _db = db;
    }
    // GET: api/<DirectorController>
    [HttpGet]
    public async Task<IResult> Get()
    {
        try
        {
            _db.Include<Film>();
            _db.IncludeRef<FilmGenre>();
            var directors = await _db.GetAsync<Director, DirectorDTO>();
            return Results.Ok(directors);
        }
        catch { }

        return Results.NotFound();
    }

    // GET api/<DirectorController>/5
    [HttpGet("{id}")]
    public async Task<IResult> Get(int id)
    {
        try
        {
            _db.Include<Film>();
            var director = await _db.SingleAsync<Director, DirectorDTO>(i => i.Id == id);
            return Results.Ok(director);
        }
        catch { }

        return Results.NotFound();
    }

    // POST api/<DirectorController>
    [HttpPost]
    public async Task<IResult> Post(DirectorCreateDTO dto)
    {
        try
        {
            if (dto == null) { return Results.BadRequest(); }

            var entity = await _db.AddAsync<Director, DirectorCreateDTO>(dto);

            if (await _db.SaveChangesAsync())
            {
                var node = typeof(Director).Name.ToLower();
                return Results.Created($"/{node}s/{entity.Id}", dto);
            }

        }
        catch { }

        return Results.BadRequest();
    }

    // PUT api/<DirectorController>/5
    [HttpPut("{id}")]
    public async Task<IResult> Put(int id, DirectorDTO dto)
    {
        try
        {
            if (dto == null || id != dto.Id) { return Results.BadRequest(); }

            if (!await _db.AnyAsync<Director>(i => i.Id.Equals(id)))
                return Results.NotFound();

            _db.Update<Director, DirectorDTO>(id, dto);

            var success = await _db.SaveChangesAsync();

            if (!success)
                return Results.BadRequest();

            return Results.NoContent();
        }
        catch { }
        return Results.NotFound();
    }

    // DELETE api/<DirectorController>/5
    [HttpDelete("{id}")]
    public async Task<IResult> Delete(int id)
    {
        try
        {
            _db.Include<Director>();
            var success = await _db.DeleteAsync<Director>(id);
            if (!success) return Results.NotFound();

            success = await _db.SaveChangesAsync();
            if (!success) return Results.BadRequest();

            return Results.NoContent();

        }
        catch { }

        return Results.BadRequest();
    }
}
