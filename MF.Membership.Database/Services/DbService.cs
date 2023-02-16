using MF.Common.DTOs;
using System.Reflection.Metadata.Ecma335;

namespace MF.Membership.Database.Services;

public class DbService : IDbService
{
    private readonly MFContext _db;
    private readonly IMapper _mapper;

    public DbService(MFContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<TDto>> GetAsync<TEntity, TDto>()
       where TEntity : class, IEntity where TDto : class
    {
        var entities = await _db.Set<TEntity>().ToListAsync();
        return _mapper.Map<List<TDto>>(entities);
    }

    private async Task<TEntity?> SingleAsync<TEntity>(Expression<Func<TEntity,
    bool>> expression) where TEntity : class, IEntity =>
    await _db.Set<TEntity>().SingleOrDefaultAsync(expression);

    public async Task<TDto> SingleAsync<TEntity, TDto>(Expression<Func<TEntity, bool>> expression)
        where TEntity : class, IEntity where TDto : class
    {
        var entity = await SingleAsync(expression);
        return _mapper.Map<TDto>(entity);
    }

    public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression)
        where TEntity : class, IEntity
    {
        return await _db.Set<TEntity>().AnyAsync(expression);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _db.SaveChangesAsync() >= 0;
    }

    public async Task<TEntity> AddAsync<TEntity, TDto>(TDto dto)
        where TEntity : class, IEntity where TDto : class
    {
        var entity = _mapper.Map<TEntity>(dto);
        await _db.Set<TEntity>().AddAsync(entity);
        return entity;
    }

    public void Update<TEntity, TDto>(int id, TDto dto)
        where TEntity : class, IEntity where TDto : class
    {
        var entity = _mapper.Map<TEntity>(dto);
        entity.Id = id;
        _db.Set<TEntity>().Update(entity);
    }

    public async Task<bool> DeleteAsync<TEntity>(int id)
        where TEntity : class, IEntity
    {
        try
        {
            var entity = await SingleAsync<TEntity>(e => e.Id.Equals(id));
            if (entity == null)
                return false;
            _db.Remove(entity);
            return true;
        }
        catch
        {
            throw;
        }
    }

    public void IncludeNew<TEntity>(int id) where TEntity : class, IEntity
    {
        var propertyNames =
            _db.Model.FindEntityType(typeof(TEntity))?.GetNavigations().Select(e => e.Name);

        if (propertyNames is null)
            return;

        foreach (var name in propertyNames)
            _db.Set<Film>()
                .Where(f => f.Id == id)
                .Include(film => film.Genres).Load();
    }
    //Adams specialare
    /*public async Task<SingleFilmDTO> GetSingleFilm(int id)
    {
        var chosenFilm = await _db.Set<Film>()
        .Where(f => f.Id == id)
        .Include(film => film.Genres)
        .Include(film => film.Director)
        .Include(film => film.SimilarFilms)
        .ThenInclude(film => film.SimilarFilm)
        .SingleOrDefaultAsync();
        return _mapper.Map<SingleFilmDTO>(chosenFilm);
    }
    public void IncludeFilmGenre(int id)
    {
        _db.Set<Film>()
        .Where(f => f.Id == id)
        .Include(film => film.Genres)
        .Load();
    }*/

    public async Task<List<FilmGenreDTO>> GetFilmsInGenreAsync(int genreId)
    {
        var films = await _db.Set<FilmGenre>()
            .Where(g => g.GenreId == genreId)
            .ToListAsync();

        return _mapper.Map<List<FilmGenreDTO>>(films);
    }

    public void Include<TEntity>() where TEntity : class, IEntity
    {
        var propertyNames =
            _db.Model.FindEntityType(typeof(TEntity))?.GetNavigations().Select(e => e.Name);

        if (propertyNames is null)
            return;

        foreach (var name in propertyNames)
            _db.Set<TEntity>().Include(name).Load();
    }

    #region ReferenceEntity


    public void IncludeRef<TEntity>() where TEntity : class, IReferenceEntity
    {
        var propertyNames =
            _db.Model.FindEntityType(typeof(TEntity))?.GetNavigations().Select(e => e.Name);

        if (propertyNames is null)
            return;

        foreach (var name in propertyNames)
            _db.Set<TEntity>().Include(name).Load();
    }

    async Task<List<TDto>> IDbService.GetRefAsync<TReferenceEntity, TDto>()
    {
        try
        {
            var entities = await _db.Set<TReferenceEntity>().ToListAsync();
            return _mapper.Map<List<TDto>>(entities);
        }
        catch
        {
            throw;
        }
    }



    private async Task<TReferenceEntity?> SingleRefAsync<TReferenceEntity>(Expression<Func<TReferenceEntity,
    bool>> expression) where TReferenceEntity : class, IReferenceEntity =>
    await _db.Set<TReferenceEntity>().SingleOrDefaultAsync(expression);

    async Task<TDto> IDbService.SingleRefAsync<TReferenceEntity, TDto>(Expression<Func<TReferenceEntity, bool>> expression) where TReferenceEntity : class
    {
        var entity = await _db.Set<TReferenceEntity>().AsNoTracking().SingleOrDefaultAsync(expression);//SingleRefAsync<TReferenceEntity>(expression);
        return _mapper.Map<TDto>(entity);
    }

    async Task<TReferenceEntity> IDbService.SingleRefEntityAsync<TReferenceEntity>(Expression<Func<TReferenceEntity, bool>> expression) where TReferenceEntity : class
    {
        return await _db.Set<TReferenceEntity>().SingleOrDefaultAsync(expression);//SingleRefAsync<TReferenceEntity>(expression);
    }

    public async Task<bool> DeleteRefAsync<TReferenceEntity, TDto>(TDto dto, int filmId, int genreId)
        where TReferenceEntity : class where TDto : class
    {
        try
        {
            var entity = _mapper.Map<TReferenceEntity>(dto);
            //if(!await AnyRefAsync<FilmGenre>(e => (e.FilmId.Equals(filmId) || (e.GenreId.Equals(genreId)))))
            //    //if(entity == null)
            //    return false;
            _db.Remove(entity);
        }
        catch
        {
            throw;
        }
        return true;
    }

    public async Task<bool> AnyRefAsync<TEntity>(Expression<Func<TEntity, bool>> expression)
        where TEntity : class, IReferenceEntity
    {
        return await _db.Set<TEntity>().AnyAsync(expression);
    }

    public async Task<TEntity> AddRefAsync<TEntity, TDto>(TDto dto)
        where TEntity : class, IReferenceEntity where TDto : class
    {
        try
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _db.Set<TEntity>().AddAsync(entity);
            return entity;
        }
        catch
        {
            throw;
        }
    }
    public void UpdateFilmGenre(int id, FilmGenreDTO dto)
        //where TEntity : class, IReferenceEntity where TDto : class
    {
        var entity = _mapper.Map<FilmGenre>(dto);
        entity.FilmId = id;
        _db.Set<FilmGenre>().Update(entity);
    }

    #endregion
}
