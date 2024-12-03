using System.Diagnostics;
using Services.Application.Domain.Interfaces;
using Services.Common.Abstractions;
using Services.Common.Model;

namespace Services.Application.Domain.Services;

public class ProcessingService : IProcessingService
{
    private readonly IAdministratorFactory _administratorFactory;
    private readonly IBus _bus;

    public ProcessingService(IAdministratorFactory administratorFactory, IBus bus)
    {
        _administratorFactory = administratorFactory;
        _bus = bus;
    }
    public async Task Process(ProductCode productCode, Common.Model.Application application)
    {
        var (serviceAdministrator, rules) = _administratorFactory.GetAdministratorAndRules(application.ProductCode);
        
        var rulesEngine = new RulesEngine(rules);
        var isValid = rulesEngine.Validate(application, out var errors);
        if (!isValid)
        {
            await _bus.PublishAsync(new ApplicationNotValid(application.Id, errors));
            return;
        }

        await (productCode switch
        {
            ProductCode.ProductOne => ProcessProductOne(application, serviceAdministrator),
            ProductCode.ProductTwo => ProcessProductTwo(application, serviceAdministrator),
            _ => throw new InvalidOperationException("Invalid product code")
        });
    }
    
    private async Task ProcessProductOne(Common.Model.Application application, IServiceAdministrator serviceAdministrator)
    {
        var investorId = await serviceAdministrator.CreateInvestorAsync(application.Applicant, application.Payment.Amount.Amount);
        
        await _bus.PublishAsync(new InvestorCreated(application.Applicant.Id, investorId));
        await _bus.PublishAsync(new ApplicationCompleted(application.Id));
    }
    
    private async Task ProcessProductTwo(Common.Model.Application application, IServiceAdministrator serviceAdministrator)
    {
        var investorId = await serviceAdministrator.CreateInvestorAsync(application.Applicant, null);
        
        await _bus.PublishAsync(new InvestorCreated(application.Applicant.Id, investorId));
        
        var accountId = await serviceAdministrator.CreateAccountAsync(application.Applicant.Id, application.ProductCode);
        
        await _bus.PublishAsync(new AccountCreated(investorId, application.ProductCode, accountId));
        
        await serviceAdministrator.ProcessPaymentAsync(Guid.Parse(accountId), application.Payment);

        await _bus.PublishAsync(new ApplicationCompleted(application.Id));
    }
}