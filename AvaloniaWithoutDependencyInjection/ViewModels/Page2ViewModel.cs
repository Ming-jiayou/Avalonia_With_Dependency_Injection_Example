using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AvaloniaWithoutDependencyInjection.ViewModels
{
    public partial class Page2ViewModel : ViewModelBase
    {
        private int countNum = 0;

        [ObservableProperty]
        private string text = "你还没点击";

        [RelayCommand]
        public void Count()
        {
            countNum++;
            Text = $"你点击了{countNum}次";
        }
    }
}
