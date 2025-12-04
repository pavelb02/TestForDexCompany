using Api.Middleware;
using Application.Services.Interfaces;
using Application.Services.Mapping;
using Application.Services.Options;
using Application.Services.Services;
using Infrastructure.Data;
using Infrastructure.Minio;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.Configure<AdvertisementOptions>(
    builder.Configuration.GetSection("AdvertisementOptions"));

builder.Services.AddDbContext<TestForDexCompanyDbContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddAutoMapper(cfg => { }, typeof(UserProfile), typeof(AdvertisementProfile));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddSingleton<MinioStorageService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger/index.html");
    return Task.CompletedTask;
});

app.UseHttpsRedirection();
app.MapControllers();

app.Run();