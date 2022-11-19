using System;
using cg_3.ViewModels;
using ReactiveUI;

namespace cg_3.Views.Windows;

#nullable disable

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IViewFor<MainViewModel>
{
    public MainWindow()
    {
        ViewModel = ViewModelLocator.MainViewModel;
        InitializeComponent();
    }

    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => throw new NotImplementedException();
    }

    public MainViewModel ViewModel { get; set; }
}

#nullable  restore