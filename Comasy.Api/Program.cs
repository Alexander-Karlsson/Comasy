
namespace Comasy.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddStatisticsServices(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            
            app.UseHttpsRedirection();

            app.MapGet("/", () => "Comasy Statistics API");

            app.MapControllers();

            app.Run();
        }
    }
}
