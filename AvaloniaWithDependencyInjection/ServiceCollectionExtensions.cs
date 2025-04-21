using AvaloniaWithDependencyInjection.ViewModels;
using AvaloniaWithDependencyInjection.Views;
using AvaloniaWithDependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaWithDependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddViews(this IServiceCollection services)
    {
        // Register all views as singletons
        services.AddSingleton<MainWindow>();
        services.AddSingleton<Page1View>();
        services.AddSingleton<Page2View>();
        
        return services;
    }
    
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        // Register all view models as singletons
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<Page1ViewModel>();
        services.AddSingleton<Page2ViewModel>();
        
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<INavigationService, NavigationService>();
        return services;
    }
} 