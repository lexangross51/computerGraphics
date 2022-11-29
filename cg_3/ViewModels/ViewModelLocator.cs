using Microsoft.Extensions.DependencyInjection;

namespace cg_3.ViewModels;

public class ViewModelLocator
{
    public PlaneViewModel PlaneViewModel => App.Current.Services.GetRequiredService<PlaneViewModel>();
}