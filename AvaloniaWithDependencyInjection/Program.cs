using System;
using Avalonia;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaWithDependencyInjection
{
    internal sealed class Program
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            
            // Register services
            services.AddViews()
                    .AddViewModels()
                    .AddServices();
            
            ServiceProvider = services.BuildServiceProvider();
            
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}
