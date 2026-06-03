using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Rosa.Core.Enums;
using Rosa.Core.Interfaces;
using Rosa.Core.Models;

namespace Rosa.UI.ViewModel;

public partial class AccountantViewModel(IRequestRepository repository, ILogger<AccountantViewModel> logger)
    : ObservableObject
{
    public ObservableCollection<CertificateRequest> AllRequests { get; } = [];
    public IEnumerable<RequestStatus> AvailableStatuses => Enum.GetValues<RequestStatus>();
    [ObservableProperty] private string _errorMessage = string.Empty;

    [RelayCommand]
    public async Task LoadAllRequestsAsync()
    {
        try
        {
            var requests = await repository.GetAll();

            AllRequests.Clear();
            foreach (var req in requests)
            {
                AllRequests.Add(req);
            }
            ErrorMessage = string.Empty;
        }
        catch (Exception ex)
        {
            ErrorMessage = "Не удалось загрузить очередь заявок.";
            logger.LogError(ex, "Ошибка при загрузке всех заявок для бухгалтера");
        }
    }

    [RelayCommand]
    public async Task UpdateRequestStatusAsync(CertificateRequest request)
    {
        if (request == null) return;

        try
        {
            await repository.Update(request);
            ErrorMessage = string.Empty;
            logger.LogInformation("Статус заявки {Id} успешно изменен на {Status}", request.Id, request.Status);

            // await LoadAllRequestsAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка при сохранении статуса для заявки {request.Id}.";
            logger.LogError(ex, "Ошибка обновления статуса заявки {Id}", request.Id);
        }
    }
}