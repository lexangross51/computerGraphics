using Microsoft.Extensions.DependencyInjection;

namespace cg_3.ViewModels;

public class ViewModelLocator
{
    public MainViewModel MainViewModel => App.Current.Services.GetRequiredService<MainViewModel>();
}