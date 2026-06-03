using Rosa.Core.Enums;
using Rosa.Core.Models;

namespace Rosa.Core.Interfaces;

public interface IRequestRepository
{
    Task<List<CertificateRequest>> GetByEmployee(string employeeName);
    Task<List<CertificateRequest>> GetAll();
    Task Add(CertificateRequest  certificateRequest);
    Task Update(CertificateRequest certificateRequest);
    Task Delete(CertificateRequest certificateRequest);
    Task<bool> HasPendingRequest(string employeeName, CertificateType certificateType);
}