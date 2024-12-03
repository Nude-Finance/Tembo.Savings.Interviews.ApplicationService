using Services.Common.Model;

namespace Services.Application.Domain.Interfaces;

public interface IServiceAdministrator
{
    Task<string> CreateInvestorAsync(User user, decimal? initialInvestment);
    Task<string> CreateAccountAsync(Guid investorId, ProductCode productCode);
    Task<string> ProcessPaymentAsync(Guid accountId, Payment payment);
}