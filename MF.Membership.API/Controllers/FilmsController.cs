using MF.Membership.Database.Entities;
using MF.Membership.Database.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MF.Membership.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly IDbService _db;

        public FilmsController(IDbService db)
        {
            _db = db;
        }
        // GET: api/<FilmsController>
        [HttpGet]
        public async Task<IResult> Get()
        {
            try
            {

                _db.Include<Film>();
                _db.IncludeRef<FilmGenre>();
                var films = await _db.GetAsync<Film, FilmDTO>();
                return Results.Ok(films);
            }
            catch { }

            return Results.NotFound();
        }

        // GET api/<FilmsController>/5
        [HttpGet("{id}")]
        public async Task<IResult> Get(int id)
        {
            try
            {
                _db.Include<Film>();
                _db.IncludeRef<FilmGenre>();
                var result = await _db.SingleAsync<Film, FilmDTO>(i => i.Id == id);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch { }

            return Results.NotFound();
        }

        // POST api/<FilmsController>
        [HttpPost]
        public async Task<IResult> Post(FilmCreateDTO dto)
        {
            try
            {
                if (dto == null) { return Results.BadRequest(); }

                var entity = await _db.AddAsync<Film, FilmCreateDTO>(dto);

                if (await _db.SaveChangesAsync())
                {
                    var node = typeof(Film).Name.ToLower();
                    return Results.Created($"/{node}s/{entity.Id}", dto);
                }

            }
            catch { }

            return Results.BadRequest();
        }

        // PUT api/<FilmsController>/5
        [HttpPut("{id}")]
        public async Task<IResult> Put(int id, FilmEditDTO dto)
        {
            try
            {
                if (dto == null || id != dto.Id)
                    return Results.BadRequest();

                if (!await _db.AnyAsync<Film>(i => i.Id.Equals(id)))
                    return Results.NotFound();

                _db.Update<Film, FilmEditDTO>(id, dto);

                var success = await _db.SaveChangesAsync();

                if (!success)
                    return Results.BadRequest();

                return Results.NoContent();
            }
            catch { }
            return Results.NotFound();
        }

        // DELETE api/<FilmsController>/5
        [HttpDelete("{id}")]
        public async Task<IResult> Delete(int id)
        {
            try
            {
                var success = await _db.DeleteAsync<Film>(id);
                if (!success)
                    return Results.NotFound();

                success = await _db.SaveChangesAsync();
                if (!success)
                    return Results.BadRequest();

                return Results.NoContent();

            }
            catch { }

            return Results.BadRequest();
        }
    }
}
