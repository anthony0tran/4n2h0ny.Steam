using _4n2h0ny.Steam.API.Configurations;
using _4n2h0ny.Steam.API.Context;
using _4n2h0ny.Steam.API.Repositories;
using _4n2h0ny.Steam.API.Repositories.Interfaces;
using _4n2h0ny.Steam.API.Repositories.Profiles;
using _4n2h0ny.Steam.API.Services;
using _4n2h0ny.Steam.API.Services.Interfaces;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<SteamConfiguration>(builder.Configuration.GetSection("SteamConfiguration"));
builder.Services.Configure<CommentConfiguration>(builder.Configuration.GetSection("CommentConfiguration"));
builder.Services.AddDbContext<ProfileContext>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<ISteamService, SteamService>();
builder.Services.AddScoped<ICommentService, CommentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
