using MF.Membership.Database.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MF.Membership.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SimilarFilmsController : ControllerBase
{
    private readonly IDbService _db;

    public SimilarFilmsController(IDbService db)
    {
        _db = db;
    }
    // GET: api/<SimilarFilmsController>
    [HttpGet]
    public async Task<IResult> Get()
    {
        try
        {
            _db.IncludeRef<SimilarFilm>();
            var films = await _db.GetRefAsync<SimilarFilm, SimilarFilmDTO>();
            return Results.Ok(films);
        }
        catch { }

        return Results.NotFound();
    }

    // GET api/<SimilarFilmsController>/5
    //[HttpGet("{parentId:int}", Name = "Get")]
    //public async Task<IResult> Get(int parentId)
    //{
    //    try
    //    {
    //        var films = await _db.GetFilmsInGenreAsync(parentId);
    //        return Results.Ok(films);
    //    }
    //    catch
    //    {
    //        return Results.BadRequest();
    //    }
    //}

    // POST api/<SimilarFilmsController>
    [HttpPost]
    public async Task<IResult> Post(SimilarFilmCreateDTO dto)
    {
        try
        {
            var entity = await _db.AddRefAsync<SimilarFilm, SimilarFilmCreateDTO>(dto);
            if (await _db.SaveChangesAsync())
            {
                return Results.Ok();
            }
        }
        catch (Exception ex)
        {
            return Results.BadRequest($"Couldn't add the {typeof(SimilarFilm).Name}" +
            $"entity.\n{ex}.");
        }
        return Results.BadRequest($"Couldn't add the {typeof(SimilarFilm).Name}" +
            $"entity.");
    }

    // PUT api/<SimilarFilmsController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<SimilarFilmsController>/5
    [HttpDelete()]
    public async Task<IResult> Delete([FromQuery] int[] id)
    {
        try
        {
            var success = await _db.DeleteRefAsync<SimilarFilm, SimilarFilmDTO>(new SimilarFilmDTO { ParentFilmId = id[0], SimilarFilmId = id[1] });

            if (!success) return Results.BadRequest();

            success = await _db.SaveChangesAsync();

            if (!success) return Results.BadRequest();


            return Results.NoContent();
        }
        catch
        {
            throw;
        }
    }
}
