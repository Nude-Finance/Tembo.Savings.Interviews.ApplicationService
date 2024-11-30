using MediatR;
using Services.AdministratorOne.Abstractions;
using Services.AdministratorOne.Abstractions.Model;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Commands.Handlers;

public class CreateInvestorCommandHandler : IRequestHandler<CreateInvestorCommand>
{
    private readonly IAdministrationService _investorService;
    private readonly IMediator _mediator;

    public CreateInvestorCommandHandler(IAdministrationService investorService, IMediator mediator)
    {
        _investorService = investorService;
        _mediator = mediator;
    }

    public async Task Handle(CreateInvestorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine($"Handling CreateInvestorCommand: ApplicationId={request.Application.Id}");

            //To do: create investor request
            var investorResult =  _investorService.CreateInvestor(new CreateInvestorRequest());

            if (!string.IsNullOrEmpty(investorResult.InvestorId) )
            {
                throw new InvalidOperationException("Failed to create investor.");
            }

            var investorId = investorResult.InvestorId;

            // Publish InvestorCreated event
            await _mediator.Publish(new InvestorCreated(request.Application.Applicant.Id, investorId), cancellationToken);
        }
        catch (Exception ex)
        {
            // Optionally trigger compensation if needed
            await _mediator.Send(new SagaCompensationCommand(request.Application.Id, "CreateInvestor", ex.Message), cancellationToken);
            throw;
        }
    }
}
