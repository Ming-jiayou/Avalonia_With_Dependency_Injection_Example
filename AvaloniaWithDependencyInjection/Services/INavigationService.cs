using AvaloniaWithDependencyInjection.ViewModels;

namespace AvaloniaWithDependencyInjection.Services;

public interface INavigationService
{
    ViewModelBase CurrentViewModel { get; }
    void NavigateTo<T>() where T : ViewModelBase;
} 