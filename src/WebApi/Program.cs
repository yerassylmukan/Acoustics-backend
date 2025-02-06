using WebApi.Common.Contracts;
using WebApi.Data;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.Logger.LogInformation("Web API created...");

app.Logger.LogInformation("Seeding Database...");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var dbContext = services.GetRequiredService<AppDbContext>();
    var userRepository = services.GetRequiredService<IUserRepository>();
    await SeedData.SeedAsync(dbContext, userRepository);
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