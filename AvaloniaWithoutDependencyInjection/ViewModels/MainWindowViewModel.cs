using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaWithoutDependencyInjection.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase currentPage;

        public MainWindowViewModel()
        {
            CurrentPage = new Page1ViewModel();
        }

        [RelayCommand]
        private void NavigateToPage1()
        {
            CurrentPage = new Page1ViewModel();
        }

        [RelayCommand]
        private void NavigateToPage2()
        {
            CurrentPage = new Page2ViewModel();
        }     
    }
}
