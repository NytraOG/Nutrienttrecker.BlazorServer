using System.Reflection;
using CoOrga.SpaceManagement;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.DesignTime;
using DevExpress.ExpressApp.Design;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.BaseImpl;
using Domain.Entities;

namespace Host.Blazor.Server;

public class Program : IDesignTimeApplicationFactory
{
    XafApplication IDesignTimeApplicationFactory.Create()
    {
        var hostBuilder = CreateHostBuilder(Array.Empty<string>());
        return DesignTimeApplicationFactoryHelper.Create(hostBuilder);
    }

    private static bool ContainsArgument(string[] args, string argument) => args.Any(arg => arg.TrimStart('/').TrimStart('-').ToLower() == argument.ToLower());

    public static int Main(string[] args)
    {

        //MachMigrations();

        if (ContainsArgument(args, "help") || ContainsArgument(args, "h"))
        {
            Console.WriteLine("Updates the database when its version does not match the application's version.");
            Console.WriteLine();
            Console.WriteLine($"    {Assembly.GetExecutingAssembly().GetName().Name}.exe --updateDatabase [--forceUpdate --silent]");
            Console.WriteLine();
            Console.WriteLine("--forceUpdate - Marks that the database must be updated whether its version matches the application's version or not.");
            Console.WriteLine("--silent - Marks that database update proceeds automatically and does not require any interaction with the user.");
            Console.WriteLine();
            Console.WriteLine($"Exit codes: 0 - {DBUpdaterStatus.UpdateCompleted}");
            Console.WriteLine($"            1 - {DBUpdaterStatus.UpdateError}");
            Console.WriteLine($"            2 - {DBUpdaterStatus.UpdateNotNeeded}");
        }
        else
        {
            FrameworkSettings.DefaultSettingsCompatibilityMode = FrameworkSettingsCompatibilityMode.Latest;
            var host = CreateHostBuilder(args).Build();

            if (ContainsArgument(args, "updateDatabase"))
            {
                using (var serviceScope = host.Services.CreateScope())
                {
                    return serviceScope.ServiceProvider.GetRequiredService<IDBUpdater>().Update(ContainsArgument(args, "forceUpdate"), ContainsArgument(args, "silent"));
                }
            }

            host.Run();
        }

        return 0;
    }

    private static void MachMigrations()
    {
        var types = typeof(BaseEntity).Assembly
                                      .GetTypes()
                                      .Union(typeof(FileData).Assembly.GetTypes())
                                      .ToList();

        var connectionString = "Integrated Security=SSPI;Pooling=false;Data Source=DESKTOP-7FA7F9C\\MSSQLSERVER2019;Initial Catalog=Nutrienttrecker_local";
        //var connectionString = "User Id=dbo954319730;Password=cG!H3n5XMy6EVfu;Pooling=false;Data Source=db954319730.hosting-data.io;Initial Catalog=db954319730";

        DatabaseOperations.RecreateDatabase(connectionString, types);
    }

    public static IHostBuilder CreateHostBuilder(string[] args) => Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                                                                            .ConfigureWebHostDefaults(webBuilder =>
                                                                             {
                                                                                 webBuilder.UseStartup<Startup>();
                                                                             });
}