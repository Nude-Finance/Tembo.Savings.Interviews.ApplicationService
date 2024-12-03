using Services.AdministratorOne.Abstractions;
using Services.AdministratorOne.Abstractions.Model;
using Services.Application.Domain.Interfaces;
using Services.Common.Model;

namespace Services.Application.Domain.Adapters;

public class AdministratorOneAdapter : IServiceAdministrator
{
    private readonly IAdministrationService _administrationService;

    public AdministratorOneAdapter(IAdministrationService administrationService)
    {
        _administrationService = administrationService;
    }

    public async Task<string> CreateInvestorAsync(User user, decimal? initialInvestment)
    {
        var request = new CreateInvestorRequest
        {
            
        };
        var response = _administrationService.CreateInvestor(request);
        
        if (response == null)
        {
            throw new InvalidOperationException("Failed to create investor");
        }
        return await Task.FromResult(response.InvestorId);
    }

    public async Task<string> CreateAccountAsync(Guid investorId, ProductCode productCode)
    {
        throw new NotImplementedException();
    }

    public async Task<string> ProcessPaymentAsync(Guid accountId, Payment payment)
    {
        throw new NotImplementedException();
    }
}