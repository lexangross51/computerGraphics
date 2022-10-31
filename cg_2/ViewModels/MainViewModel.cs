using ReactiveUI;

namespace cg_2.ViewModels;

public class MainViewModel : ReactiveObject
{
    public DrawingViewModel DrawingViewModel { get; }

    public MainViewModel(DrawingViewModel drawingViewModel) => DrawingViewModel = drawingViewModel;
}