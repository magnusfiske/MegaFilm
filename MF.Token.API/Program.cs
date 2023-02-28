

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(policy =>
{
    policy.AddPolicy("CorsAllAccessPolicy", opt =>
        opt.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
        );
});

builder.Services.AddDbContext<MFUserContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("MFUserConnection")));

builder.Services.AddIdentity<MFUser, IdentityRole>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MFUserContext>()
    .AddDefaultTokenProviders();

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
