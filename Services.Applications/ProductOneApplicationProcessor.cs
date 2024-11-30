using Services.AdministratorOne.Abstractions;
using Services.AdministratorOne.Abstractions.Model;
using Services.Common.Abstractions.Abstractions;
using Services.Common.Abstractions.Model;
using Services.Common.Abstractions.Model.Products;

namespace Services.Applications;

public class ProductOneApplicationProcessor(IAdministrationService administratorOneService)
    : IApplicationProcessor<ProductOne>
{
    private readonly IAdministrationService _administratorOneService = administratorOneService;

    public Task<Result> Process(Application<ProductOne> application)
    {
        var createInvestorRequest = new CreateInvestorRequest();
        var createInvestorResponse = _administratorOneService.CreateInvestor(createInvestorRequest);
        
        return Task.FromResult(Result.Success());
    }
}