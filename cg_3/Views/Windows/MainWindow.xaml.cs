using System.Reactive.Disposables;
using System.Reactive.Linq;
using cg_3.Source.Render;
using cg_3.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using OpenTK.Wpf;
using ReactiveUI;

namespace cg_3.Views.Windows;

#nullable disable

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IViewFor<MainViewModel>
{
    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => throw new NotImplementedException();
    }

    public MainViewModel ViewModel { get; set; }
    public IBaseGraphic BaseGraphic { get; }
    
    public MainWindow()
    {
        InitializeComponent();
        ViewModel = App.Current.Services.GetRequiredService<MainViewModel>();
        var mainSettings = new GLWpfControlSettings();
        OpenTkControl.Start(mainSettings);
        OpenTkControl.RenderSize = new(1920, 1080);

        BaseGraphic = new RenderServer();
        ViewModel.Draw(BaseGraphic); // initialize
        
        var observable = Observable.FromEvent<TimeSpan>(
            handler => OpenTkControl.Render += handler,
            handler => OpenTkControl.Render -= handler);

        this.WhenActivated(disposables 
            => observable.Subscribe(ts => BaseGraphic.Render(ts)).DisposeWith(disposables));
    }
}

#nullable  restore