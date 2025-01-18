using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using WebApi.Data;
using WebApi.Domain;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureAppDbContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureSwaggerServices();
builder.Services.ConfigureCorsPolicy();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddLogging();

var app = builder.Build();

app.Logger.LogInformation("Web API created...");

app.Logger.LogInformation("Seeding Database...");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var dbContext = services.GetRequiredService<AppDbContext>();
    await SeedData.SeedAsync(dbContext, userManager, roleManager);
}
catch (Exception e)
{
    app.Logger.LogError(e, "An error occurred while seeding the database.");
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
app.MapControllers();
app.Run();

public partial class Program
{
}