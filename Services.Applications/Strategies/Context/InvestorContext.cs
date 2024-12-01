using Microsoft.Extensions.DependencyInjection;
using Services.Applications.Strategies.Abstractions;
using Services.Applications.Strategies.InvestorStrategy;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Strategies.Context;

public class InvestorContext(IServiceProvider serviceProvider)
{
    public IInvestorCreator GetInvestorCreator(ProductCode productCode)
    {
        return productCode switch
        {
            ProductCode.ProductOne => serviceProvider.GetRequiredService<ProductOneInvestorCreator>(),
            ProductCode.ProductTwo => serviceProvider.GetRequiredService<ProductTwoInvestorCreator>(),
            _ => throw new NotSupportedException($"Unsupported ProductCode: {productCode}")
        };
    }
}