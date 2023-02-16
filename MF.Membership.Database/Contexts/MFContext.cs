using Microsoft.EntityFrameworkCore;

namespace MF.Membership.Database.Contexts;

public class MFContext : DbContext
{
    public MFContext(DbContextOptions<MFContext> options) : base(options)
    {

    }
    public virtual DbSet<Film> Films => Set<Film>();
    public virtual DbSet<Genre> Genres => Set<Genre>();
    public virtual DbSet<Director> Directors => Set<Director>();
    public virtual DbSet<FilmGenre> FilmGenres => Set<FilmGenre>();
    public virtual DbSet<SimilarFilm> SimilarFilms => Set<SimilarFilm>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        /* Composit Keys */
        builder.Entity<SimilarFilm>().HasKey(ci => new { ci.ParentFilmId, ci.SimilarFilmId });
        builder.Entity<FilmGenre>().HasKey(ci => new { ci.FilmId, ci.GenreId });

        /* Configuring related tables for the Film table*/
        builder.Entity<Film>(entity =>
          {
              entity
                  // For each film in the Film Entity,
                  // reference relatred films in the SimilarFilms entity
                  // with the ICollection<SimilarFilms>
                  .HasMany(d => d.SimilarFilms)
                  .WithOne(p => p.Film)
                  .HasForeignKey(d => d.ParentFilmId)
                  // To prevent cycles or multiple cascade paths.
                  // Neded when seeding similar films data.
                  .OnDelete(DeleteBehavior.ClientSetNull);


              // Configure a many-to-many realtionship between genres
              // and films using the FilmGenre connection entity.
              entity.HasMany(d => d.Genres)
                    .WithMany(p => p.Films)
                    .UsingEntity<FilmGenre>()
                    // Specify the table name for the connection table
                    // to avoid duplicate tables (FilmGenre and FilmGenres)
                    // in the database.
                    .ToTable("FilmGenres");
          });

        builder.Entity<Director>(entity =>
        {
            entity
                .HasMany(f => f.Films)
                .WithOne(p => p.Director)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}
