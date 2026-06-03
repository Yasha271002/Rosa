
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Rosa.UI.ViewModel;

public partial class MainViewModel(IServiceProvider serviceProvider, ILogger<MainViewModel> logger)
    : ObservableObject
{
    [ObservableProperty]
    private ObservableObject _currentViewModel = null!;

    [RelayCommand]
    private void Loaded()
    {
        NavigateToEmployee();
    }

    [RelayCommand]
    private void NavigateToEmployee()
    {
        logger.LogInformation("Переход на экран сотрудника");
        CurrentViewModel = serviceProvider.GetRequiredService<EmployeeViewModel>();
    }

    [RelayCommand]
    private void NavigateToAccountant()
    {
        logger.LogInformation("Переход на экран бухгалтера");
        CurrentViewModel = serviceProvider.GetRequiredService<AccountantViewModel>();
    }
}