using Services.Common.Model;

namespace Services.Common.Abstractions;

public interface IKycService
{
    Task<Result<KycReport>> GetKycReportAsync(User user);
}