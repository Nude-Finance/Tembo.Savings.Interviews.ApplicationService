using Services.Application.Domain.Adapters;
using Services.Application.Domain.Interfaces;
using Services.Application.Domain.Rules;
using Services.Common.Model;

namespace Services.Application.Domain;

public class AdministratorFactory : IAdministratorFactory
{
    private readonly AdministratorOneAdapter _administratorOneAdapter;
    private readonly AdministratorTwoAdapter _administratorTwoAdapter;

    public AdministratorFactory(AdministratorOneAdapter administratorOneAdapter, AdministratorTwoAdapter administratorTwoAdapter)
    {
        _administratorOneAdapter = administratorOneAdapter;
        _administratorTwoAdapter = administratorTwoAdapter;
    }

    public (IServiceAdministrator, IEnumerable<IRule>) GetAdministratorAndRules(ProductCode productCode)
    {
        return productCode switch
        {
            ProductCode.ProductOne => (_administratorOneAdapter, new List<IRule>
            {
                new AgeRule(18, 39),
                new PaymentAmountRule(new Money("GBP", 0.99m))
            }),
            ProductCode.ProductTwo => (_administratorTwoAdapter, new List<IRule>
            {
                new AgeRule(18, 200),
                new PaymentAmountRule(new Money("GBP", 0.99m))
            }),
            _ => throw new ArgumentException("Invalid product code")
        };
    }
}