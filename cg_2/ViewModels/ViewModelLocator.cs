using Microsoft.Extensions.DependencyInjection;

namespace cg_2.ViewModels;

public class ViewModelLocator
{
    public MainViewModel MainViewModel => App.Current.Services.GetRequiredService<MainViewModel>();
}