# 在Avalonia/C#中使用依赖注入过程记录

## 前言

使用依赖注入可以让我们的程序变得更加好维护与测试。

今天分享的是在Avalonia/C#中使用依赖注入。

我准备了一个简单的不使用依赖注入与使用依赖注入的demo。

该demo已上传至GitHub，地址：https://github.com/Ming-jiayou/Avalonia_With_Dependency_Injection_Example

因此本文中不会包含全部代码，有需要可以从GitHub获取全部代码。

## 实践

先运行一下AvaloniaWithoutDependencyInjection这个例子。

效果：

![](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/%E4%B8%8D%E4%BD%BF%E7%94%A8%E4%BE%9D%E8%B5%96%E6%B3%A8%E5%85%A5%E7%9A%84%E6%95%88%E6%9E%9C.gif)

虽然我们实现了导航的功能，但是当重新点击的时候又会创建一个新的实例，并不会保留之前的状态，很多时候这不是我们想要的效果。

现在再来运行一下AvaloniaWithDependencyInjection这个例子。

效果：

![](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/%E4%BD%BF%E7%94%A8%E4%BE%9D%E8%B5%96%E6%B3%A8%E5%85%A5%E7%9A%84%E6%95%88%E6%9E%9C.gif)

由于我们以单例的形式将View与ViewModel注入了依赖注入容器中了，因此你可以看到现在再重新点击是会保留之前的状态了。

现在让我们一起看看如何将上面的那个例子改造成下面的那个例子吧！！

要实现依赖注入首先需要有一个依赖注入容器，我这里使用的是Microsoft.Extensions.DependencyInjection。

为了方便实现导航，我们创建一个INavigationService接口与NavigationService类。

INavigationService：

```csharp
public interface INavigationService
{
    ViewModelBase CurrentViewModel { get; }
    void NavigateTo<T>() where T : ViewModelBase;
} 
```

NavigationService：

```csharp
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
```

为了方便添加服务，创建一个ServiceCollectionExtensions类。

```csharp
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
```

在Program中注册服务：

![image-20250421161136598](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250421161136598.png)

在App.axaml.cs中从容器中取出MainWindow与MainWindowViewModel：

![image-20250421161251620](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250421161251620.png)

在ViewLocator中从容器中取出View的实例：

![image-20250421161440244](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250421161440244.png)

MainWindowViewModel：

![image-20250421161628346](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250421161628346.png)

经过以上步骤就成功在Avalonia中实现依赖注入了，现在来看看流程是怎么样的。

## 流程

程序启动在Program中注册了服务。

App.axaml.cs中取出了MainWindow与MainWindowViewModel。

在MainWindowViewModel中注入了INavigationService。

但是在这里你可能会有疑问：

![image-20250421161943776](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250421161943776.png)

为什么这里可以直接使用`serviceProvider`呢？

看起来我们直接使用了` serviceProvider`，但实际上这里涉及到了依赖注入容器的生命周期和服务解析顺序。

当我们调用 `services.BuildServiceProvider()` 时，依赖注入容器会：

- 创建一个服务提供者（`ServiceProvider`）实例

- 这个 `ServiceProvider `包含了所有注册的服务的信息和创建规则

当需要 `NavigationService `时，依赖注入容器会：

- 发现需要创建 `NavigationService`
- 看到 `NavigationService `的构造函数需要一个 `IServiceProvider`
- 将自己（`serviceProvider`）作为参数传入
- 创建 `NavigationService` 实例

在某处第一次请求` INavigationService `时发生的：

```csharp
var navigationService = serviceProvider.GetService<INavigationService>();
```

此时：

- `serviceProvider `已经是完全初始化好的实例

- 所有的 `ViewModel `都已经注册完成

- 当调用` NavigateTo<Page1ViewModel>`() 时，可以成功从容器中解析出 `Page1ViewModel`

然后初始导航到`Page1ViewModel`。

![image-20250421162952745](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250421162952745.png)

从容器中取出Page1ViewModel并赋值给CurrentViewModel。

当CurrentViewModel改变的时候，会触发MainWindowViewModel订阅的这个属性变更事件：

![image-20250421163436887](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250421163436887.png)

如果是CurrentViewModel属性发生变化，就将MainWindowViewModel中的CurrentPage属性赋值为CurrentViewModel。

CurrentPage类型为ViewModelBase，它的变化会触发ViewLocator中的Build方法：

![image-20250421163718003](https://mingupupup.oss-cn-wuhan-lr.aliyuncs.com/imgs/image-20250421163718003.png)

该方法会根据CurrentPage的类型从容器中取出对应的View，从而实现了导航的功能，并且能够保留之前的状态。

## 最后

以上就是在Avalonia/C#中使用依赖注入的过程，希望对你有所帮助。
