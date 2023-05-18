using ShoppingListApp.Data;

namespace ShoppingListApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((hostContext, services) =>
                    {
                        services.AddControllers();
                        services.AddDbContext<AppDbContext>();
                    });

                    webBuilder.Configure(app =>
                    {
                        app.UseRouting();

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    });
                })
                .Build();

            host.Run();
        }
    }
}


