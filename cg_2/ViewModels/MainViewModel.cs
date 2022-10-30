using CommunityToolkit.Mvvm.ComponentModel;

namespace cg_2.ViewModels;

public class MainViewModel : ObservableObject
{
    public GlControlViewModel GlControlViewModel { get; }

    public MainViewModel(GlControlViewModel glControlViewModel)
    {
        GlControlViewModel = glControlViewModel;
    }
}