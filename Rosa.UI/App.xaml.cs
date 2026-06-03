using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Windows;
using Rosa.Data;
using Rosa.UI.ViewModel;

namespace Rosa.UI;
public partial class App : Application
{
    public IServiceProvider Services { get; }

    public App()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs/rosa_log_.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        Services = ConfigureServices();
        InitializeDatabase();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<MainWindow>();
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(dispose: true);
        });
        services.AddRosaData("Data Source=request.db");

        services.AddSingleton<MainViewModel>();
        services.AddTransient<EmployeeViewModel>();
        services.AddTransient<AccountantViewModel>();

        services.AddTransient<MainWindow>(provider => new MainWindow
        {
            DataContext = provider.GetRequiredService<MainViewModel>()
        });

        return services.BuildServiceProvider();
    }

    private void InitializeDatabase()
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<RosaDbContext>();
        context.Database.EnsureCreated();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Log.CloseAndFlush();
        base.OnExit(e);
    }
}