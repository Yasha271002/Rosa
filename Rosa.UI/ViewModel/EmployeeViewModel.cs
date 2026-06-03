using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Rosa.Core.Enums;
using Rosa.Core.Interfaces;
using Rosa.Core.Models;

namespace Rosa.UI.ViewModel;

public partial class EmployeeViewModel(IRequestRepository repository, ILogger<EmployeeViewModel> logger)
    : ObservableObject
{
    [ObservableProperty] private string _currentEmployeeName = "Иванов Иван Иванович";
    [ObservableProperty] private CertificateType _selectedType = CertificateType.Ndfl2;
    [ObservableProperty] private int _quantity = 1;
    [ObservableProperty] private string _reason = string.Empty;
    [ObservableProperty] private string _errorMessage = string.Empty;

    public ObservableCollection<CertificateRequest> MyRequests { get; } = [];
    public IEnumerable<CertificateType> CertificateTypes => Enum.GetValues<CertificateType>();

    [RelayCommand]
    public async Task LoadMyRequestsAsync()
    {
        try
        {
            var requests = await repository.GetByEmployee(CurrentEmployeeName);

            MyRequests.Clear();
            foreach (var req in requests)
            {
                MyRequests.Add(req);
            }

            ErrorMessage = string.Empty;
        }
        catch (Exception ex)
        {
            ErrorMessage = "Ошибка при загрузке списка заявок.";
            logger.LogError(ex, "Ошибка загрузки списка заявок для {EmployeeName}", CurrentEmployeeName);
        }
    }

    [RelayCommand]
    public async Task SubmitRequestAsync()
    {
        ErrorMessage = string.Empty;

        if (Quantity <= 0)
        {
            ErrorMessage = "Количество экземпляров должно быть больше нуля.";
            return;
        }

        try
        {
            bool hasPending = await repository.HasPendingRequest(CurrentEmployeeName, SelectedType);
            if (hasPending)
            {
                ErrorMessage = $"У вас уже есть активная заявка на справку типа {SelectedType}.";
                logger.LogWarning("Попытка создания дублирующей заявки от {EmployeeName}", CurrentEmployeeName);
                return;
            }

            var newRequest = new CertificateRequest
            {
                EmployeeName = CurrentEmployeeName,
                CertificateType = SelectedType,
                Quantity = Quantity,
                Reason = Reason
            };

            await repository.Add(newRequest);
            logger.LogInformation("Заявка успешно создана");

            Quantity = 1;
            Reason = string.Empty;
            await LoadMyRequestsAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = "Произошла ошибка при отправке заявки. Попробуйте позже.";
            logger.LogError(ex, "Ошибка при сохранении заявки");
        }
    }

    partial void OnCurrentEmployeeNameChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            MyRequests.Clear();
            return;
        }
        LoadMyRequestsCommand.Execute(null);
    }
}