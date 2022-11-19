using Microsoft.Extensions.DependencyInjection;

namespace cg_3.ViewModels;

public static class ViewModelLocator
{
    public static MainViewModel MainViewModel => App.Current.Services.GetRequiredService<MainViewModel>();
}