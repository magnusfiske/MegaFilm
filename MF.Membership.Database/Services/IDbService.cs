using MF.Common.DTOs;

namespace MF.Membership.Database.Services;

public interface IDbService
{
    Task<List<TDto>> GetAsync<TEntity, TDto>()
       where TEntity : class, IEntity where TDto : class;

    Task<TDto> SingleAsync<TEntity, TDto>(Expression<Func<TEntity, bool>> expression)
        where TEntity : class, IEntity where TDto : class;

    Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression)
        where TEntity : class, IEntity;

    Task<bool> SaveChangesAsync();

    Task<TEntity> AddAsync<TEntity, TDto>(TDto dto)
        where TEntity : class, IEntity where TDto : class;

    void Update<TEntity, TDto>(int id, TDto dto)
        where TEntity : class, IEntity where TDto : class;

    Task<bool> DeleteAsync<TEntity>(int id) where TEntity : class, IEntity;

    void Include<TEntity>() where TEntity : class, IEntity;

    public void IncludeRef<TEntity>() where TEntity : class, IReferenceEntity;


   Task<List<TDto>> GetRefAsync<TReferenceEntity, TDto>()
        where TReferenceEntity : class, IReferenceEntity where TDto : class;

    Task<TEntity> AddRefAsync<TEntity, TDto>(TDto dto) where TEntity : class, IReferenceEntity where TDto : class;

    Task<bool> DeleteRefAsync<TReferenceEntity, TDto>(TDto dto, int filmId, int genreId)
       where TReferenceEntity : class where TDto : class;

    Task<bool> AnyRefAsync<TEntity>(Expression<Func<TEntity, bool>> expression)
        where TEntity : class, IReferenceEntity;

    void UpdateFilmGenre(int id, FilmGenreDTO dto);
    void IncludeNew<TEntity>(int id) where TEntity : class, IEntity;
    Task<TDto> SingleRefAsync<TReferenceEntity, TDto>(Expression<Func<TReferenceEntity, bool>> expression)
        where TReferenceEntity : class, IReferenceEntity;
    Task<TReferenceEntity> SingleRefEntityAsync<TReferenceEntity>(Expression<Func<TReferenceEntity, bool>> expression)
        where TReferenceEntity : class;
    Task<List<FilmGenreDTO>> GetFilmsInGenreAsync(int genreId);
}
