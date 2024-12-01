using MediatR;
using Services.Applications.Strategies.Context;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Commands.Handlers;

public class CreateInvestorCommandHandler(InvestorContext investorContextFactory, IMediator mediator)
    : IRequestHandler<CreateInvestorCommand>
{
    public async Task Handle(CreateInvestorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine(
                $"Handling CreateInvestorCommand for ApplicationId={request.Application.Id}, ProductCode={request.Application.ProductCode}");
            var creator = investorContextFactory.GetInvestorCreator(request.Application.ProductCode);
            var investorId = await creator.CreateInvestorAsync(request.Application);
            Console.WriteLine($"Investor created: InvestorId={investorId}");
            if (request.Application.ProductCode == ProductCode.ProductOne)
            {
                await mediator.Send(
                    new ApplicationProcessCompleteCommand(request.Application.Id, request.Application.Applicant.Id,
                        null, ApplicationProcessStatus.Success), cancellationToken);
            }
            else
            {
                await mediator.Send(new CreateAccountCommand(
                    investorId,
                    request.Application.ProductCode,
                    request.Application.Id,
                    request.Application.Payment), cancellationToken);    
            }

            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CreateInvestorCommandHandler: {ex.Message}");
            await mediator.Send(new SagaCompensationCommand(request.Application.Id, "CreateInvestor", ex.Message),
                cancellationToken);
            throw;
        }
    }
}