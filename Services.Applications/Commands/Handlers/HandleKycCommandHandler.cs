using MediatR;
using Services.Common.Abstractions.Abstractions;

namespace Services.Applications.Commands.Handlers;

public class HandleKycCommandHandler(IKycService kycService, IMediator mediator) : IRequestHandler<HandleKycCommand>
{
    public async Task Handle(HandleKycCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine($"Handling KYC for ApplicationId={request.Application.Id}");

            var kycReport = await kycService.GetKycReportAsync(request.Application.Applicant);
            if (!kycReport.IsSuccess)
            {
                await mediator.Send(new HandleKycFailureCommand(request.Application.Applicant.Id, request.Application.Id, kycReport.Value.Id), cancellationToken);
            }
            else
            {
                Console.WriteLine($"KYC passed for ApplicationId={request.Application.Id}");
                await mediator.Send(new CreateInvestorCommand(request.Application), cancellationToken);   
            }
        }
        catch (Exception ex)
        {
            await mediator.Send(new SagaCompensationCommand(request.Application.Id, "HandleKyc", ex.Message), cancellationToken);
            Console.WriteLine($"KYC validation process failed {ex.Message}");
        }
    }
}

