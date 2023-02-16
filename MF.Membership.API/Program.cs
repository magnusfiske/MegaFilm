using MF.Membership.Database.Contexts;
using MF.Membership.Database.Services;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(); //.AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(policy => {
    policy.AddPolicy("CorsAllAccessPolicy", opt =>
        opt.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
    );
});

builder.Services.AddDbContext<MFContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("MFConnection")));


var config = new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.CreateMap<Film, FilmDTO>()
    .ForMember(dest => dest.Genres, src => src.MapFrom(s => s.Genres.Select(y => y.Name)))
    .ForMember(dest => dest.SimilarFilms, src => src.MapFrom(s => s.SimilarFilms.Select(y => y.Similar.Title)))
    .ForMember(dest => dest.Director, src => src.MapFrom(s => s.Director))
    .ReverseMap()
    .ForMember(dest => dest.Director, src => src.Ignore())
    .ForMember(dest => dest.Genres, src => src.Ignore());
    cfg.CreateMap<FilmCreateDTO, Film>().ReverseMap();
    cfg.CreateMap<FilmEditDTO, Film>().ReverseMap();
    cfg.CreateMap<Film, FilmDirectorDTO>()
    .ForMember(dest => dest.Genres, src => src.MapFrom(s => s.Genres.Select(y => y.Name)))
    .ForMember(dest => dest.SimilarFilms, src => src.MapFrom(s => s.SimilarFilms.Select(y => y.Similar.Title)));
    cfg.CreateMap<Director, DirectorDTO>()
    .ForMember(dest => dest.Films, src => src.MapFrom(s => s.Films))
    .ReverseMap()
    .ForMember(dest => dest.Films, src => src.Ignore());
    cfg.CreateMap<DirectorCreateDTO, Director>().ReverseMap();
    cfg.CreateMap<Genre, GenreDTO>()
    .ForMember(dest => dest.Films, src => src.MapFrom(s => s.Films))//.Select(f => f.Title)))
    .ReverseMap()
    .ForMember(dest => dest.Films, src => src.Ignore());
    cfg.CreateMap<GenreCreateDTO, Genre>();
    cfg.CreateMap<GenreEditDTO, Genre>();
    cfg.CreateMap<FilmGenre, FilmGenreDTO>().ReverseMap();
    cfg.CreateMap<SimilarFilm, SimilarFilmDTO>().ReverseMap();
    cfg.CreateMap<SimilarFilmCreateDTO, SimilarFilm>();
});
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

RegisterServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsAllAccessPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();


void RegisterServices(IServiceCollection services)
{
    services.AddScoped<IDbService, DbService>();
}