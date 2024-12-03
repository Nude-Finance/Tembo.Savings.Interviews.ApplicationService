using Services.Application.Domain.Interfaces;
using Services.Common.Abstractions;
using Services.Common.Model;

namespace Services.Applications;

public class ApplicationProcessor(IKycService kycService, IBus bus, IProcessingService processingService)
    : IApplicationProcessor
{
    public async Task Process(Common.Model.Application application)
    {
        var kycReport = await kycService.GetKycReportAsync(application.Applicant);
        if (!kycReport.IsSuccess)
        {
            await bus.PublishAsync(new KycFailed(application.Applicant.Id, Guid.Empty));
            return;
        }
        
        if (!kycReport.Value.IsVerified)
        {
            await bus.PublishAsync(new KycFailed(application.Applicant.Id, kycReport.Value.Id));
            return;
        }

        await processingService.Process(application.ProductCode, application);
    }
}