using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rosa.Core.Enums;
using Rosa.Core.Interfaces;
using Rosa.Core.Models;
using Rosa.Data.Utilities;

namespace Rosa.Data.Repository;

public class RequestRepository(RosaDbContext context, ILogger<RequestRepository> logger) : IRequestRepository
{
    public Task<List<CertificateRequest>> GetByEmployee(string employeeName)
        => logger.RequestProcessingWithErrorHandling(
            () => context.Request
                .Where(r => r.EmployeeName == employeeName)
                .OrderBy(r => r.CreatedAt)
                .ToListAsync(),
            $"Ошибка при получении заявок для сотрудника: {employeeName}");

    public Task<List<CertificateRequest>> GetAll()
        => logger.RequestProcessingWithErrorHandling(
            () => context.Request
                .OrderBy(r => r.CreatedAt)
                .ToListAsync(),
            "Ошибка при загрузке общего списка заявок");

    public Task Add(CertificateRequest certificateRequest)
        => logger.RequestProcessingWithErrorHandling(async () =>
        {
            await context.AddAsync(certificateRequest);
            await context.SaveChangesAsync();
        }, $"Ошибка при добавлении новой заявки для {certificateRequest.EmployeeName}");

    public Task Update(CertificateRequest certificateRequest)
        => logger.RequestProcessingWithErrorHandling(async () =>
        {
            certificateRequest.UpdatedAt = DateTime.UtcNow;
            context.Request.Update(certificateRequest);
            await context.SaveChangesAsync();
        }, $"Ошибка при обновлении статуса заявки с ID: {certificateRequest.Id}");

    public Task Delete(CertificateRequest certificateRequest)
        => logger.RequestProcessingWithErrorHandling(async () =>
        {
            context.Request.Remove(certificateRequest);
            await context.SaveChangesAsync();
        }, $"Ошибка при удалении заявки с ID: {certificateRequest.Id}");

    public Task<bool> HasPendingRequest(string employeeName, CertificateType certificateType)
        => logger.RequestProcessingWithErrorHandling(
            () => context.Request.AnyAsync(r =>
                r.EmployeeName == employeeName &&
                r.CertificateType == certificateType &&
                (r.Status == RequestStatus.New || r.Status == RequestStatus.InProgress)),
            $"Ошибка при проверке наличия активных заявок для {employeeName}");
}