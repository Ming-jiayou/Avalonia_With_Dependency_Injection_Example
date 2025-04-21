using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using AvaloniaWithDependencyInjection.ViewModels;

namespace AvaloniaWithDependencyInjection.Services;

public partial class NavigationService : ObservableObject, INavigationService
{
    [ObservableProperty]
    private ViewModelBase _currentViewModel;

    private readonly IServiceProvider _serviceProvider;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        // 设置初始页面
        NavigateTo<Page1ViewModel>();
    }

    public void NavigateTo<T>() where T : ViewModelBase
    {
        var viewModel = _serviceProvider.GetRequiredService<T>();
        CurrentViewModel = viewModel;
    }
} 