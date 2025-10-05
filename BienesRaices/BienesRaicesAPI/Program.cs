using Application;
using Application.Statics.Configurations;
using BienesRaicesAPI;
using BienesRaicesAPI.Extensions;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApiServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();


var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BienesRaicesAPI v1");
});

app.UseCors(ApiAuthSettings.CorsPolicyName);

app.UseHttpsRedirection();

app.UseErrorHandlingMiddleware();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
