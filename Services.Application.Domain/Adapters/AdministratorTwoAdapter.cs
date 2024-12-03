using Services.AdministratorTwo.Abstractions;
using Services.Application.Domain.Interfaces;
using Services.Common.Model;

namespace Services.Application.Domain.Adapters;

public class AdministratorTwoAdapter : IServiceAdministrator
{
    private readonly IAdministrationService _administrationService;

    public AdministratorTwoAdapter(IAdministrationService administrationService)
    {
        _administrationService = administrationService;
    }

    public async Task<string> CreateInvestorAsync(User user, decimal? initialInvestment)
    {
        var result = await _administrationService.CreateInvestorAsync(user);
        return CheckAndReturn(result, "Failed to create investor");
    }

    public async Task<string> CreateAccountAsync(Guid investorId, ProductCode productCode)
    {
        var result = await _administrationService.CreateAccountAsync(investorId, productCode);
        return CheckAndReturn(result, "Failed to create account");
    }

    public async Task<string> ProcessPaymentAsync(Guid accountId, Payment payment)
    {
        var result = await _administrationService.ProcessPaymentAsync(accountId, payment);
        return CheckAndReturn(result, "Failed to process payment");
    }

    private static string CheckAndReturn(Result<Guid> result, string message)
    {
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException(message);
        }

        return result.Value.ToString();
    }
}