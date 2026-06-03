using Rosa.Core.Enums;

namespace Rosa.Core.Models;

public class CertificateRequest
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string EmployeeName { get; init; } = string.Empty;
    public string? Reason { get; init; }
    public CertificateType CertificateType { get; init; }
    public int Quantity { get; init; } = 1;
    public RequestStatus Status { get; set; } = RequestStatus.New;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}