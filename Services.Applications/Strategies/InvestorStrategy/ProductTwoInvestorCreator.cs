using Services.Applications.Strategies.Abstractions;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Strategies.InvestorStrategy;

public class ProductTwoInvestorCreator(
    Services.AdministratorTwo.Abstractions.IAdministrationService administrationService)
    : IInvestorCreator
{
    public async Task<Guid> CreateInvestorAsync(Application application)
    {
        var result = await administrationService.CreateInvestorAsync(application.Applicant);

        if (!result.IsSuccess)
        {
            throw new InvalidOperationException("Failed to create investor for ProductTwo.");
        }

        return result.Value;
    }
}
