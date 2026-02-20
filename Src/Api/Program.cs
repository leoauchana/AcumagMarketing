using Api.Middlewares;
using Application;
using Data;
using Microsoft.Extensions.Options;
using Transversal;
using Transversal.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<JwtOptions>()
    .Bind(builder.Configuration.GetSection(JwtOptions.Section))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddOptions<DatabaseOptions>()
    .Bind(builder.Configuration.GetSection(DatabaseOptions.Section))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddOptions<CorsOptions>()
    .Bind(builder.Configuration.GetSection(CorsOptions.Section))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddDataServices();
builder.Services.AddTransversalServices();
var corsOptions = builder.Configuration.GetSection("Cors").Get<CorsOptions>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(corsOptions!.AllowedOrigins);
    });
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.Run();