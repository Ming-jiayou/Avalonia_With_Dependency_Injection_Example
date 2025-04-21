using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AvaloniaWithDependencyInjection.Services;

namespace AvaloniaWithDependencyInjection.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ViewModelBase currentPage;

        public MainWindowViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            CurrentPage = _navigationService.CurrentViewModel;

            // 订阅导航服务的属性变化
            ((NavigationService)_navigationService).PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(INavigationService.CurrentViewModel))
                {
                    CurrentPage = _navigationService.CurrentViewModel;
                }
            };
        }

        [RelayCommand]
        private void NavigateToPage1()
        {
            _navigationService.NavigateTo<Page1ViewModel>();
        }

        [RelayCommand]
        private void NavigateToPage2()
        {
            _navigationService.NavigateTo<Page2ViewModel>();
        }
    }
}
