using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AvaloniaWithDependencyInjection.ViewModels;
using AvaloniaWithDependencyInjection.Views;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaWithDependencyInjection
{
    public class ViewLocator : IDataTemplate
    {
        public Control? Build(object? param)
        {
            if (param is null)
                return null;

            var viewModelType = param.GetType();
            var viewTypeName = viewModelType.FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
            var viewType = Type.GetType(viewTypeName);

            if (viewType != null)
            {
                // Try to get the view from the DI container
                var view = Program.ServiceProvider?.GetService(viewType) as Control;
                if (view != null)
                {
                    return view;
                }

                // Fallback to creating a new instance if the view is not registered
                return (Control)Activator.CreateInstance(viewType)!;
            }

            return new TextBlock { Text = "Not Found: " + viewTypeName };
        }

        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
    }
}
