using System.Text.Json.Serialization;
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