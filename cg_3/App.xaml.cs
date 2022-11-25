﻿using System;
using System.Windows;
using cg_3.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace cg_3;

////// <summary>
/// Interaction logic for App.xaml
////// </summary>
public partial class App
{
    /// <summary>
    /// Gets the current <see cref="App"/> instance in use
    /// </summary>
    public new static App Current => (App)Application.Current;

    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<MainViewModel>();
        // services.AddTransient<IBaseGraphic, RenderServer>();

        return services.BuildServiceProvider();
    }

    public App()
    {
        Services = ConfigureServices();
        InitializeComponent();
    }
}