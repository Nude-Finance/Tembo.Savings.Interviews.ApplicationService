using MediatR;

namespace Services.Applications.Commands.Handlers;

public class SagaCompensationCommandHandler : IRequestHandler<SagaCompensationCommand>
{
    public async Task Handle(SagaCompensationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine($"Handling SagaCompensationCommand: ApplicationId={request.ApplicationId}, FailedStep={request.FailedStep}");

            // Perform rollback or compensation based on the failed step
            switch (request.FailedStep)
            {
                case "HandleKyc":
                    Console.WriteLine($"Rolling back KYC handling for ApplicationId={request.ApplicationId}");
                    await RollbackKycHandlingAsync(request.ApplicationId, request.Reason, cancellationToken);
                    break;

                case "HandleKycFailure":
                    Console.WriteLine($"Compensating for KYC failure handling for ApplicationId={request.ApplicationId}");
                    await CompensateKycFailureHandlingAsync(request.ApplicationId, request.Reason, cancellationToken);
                    break;

                case "CreateInvestor":
                    Console.WriteLine($"Rolling back investor creation for ApplicationId={request.ApplicationId}");
                    await RollbackInvestorCreationAsync(request.ApplicationId, cancellationToken);
                    break;

                case "CreateAccount":
                    Console.WriteLine($"Rolling back account creation for ApplicationId={request.ApplicationId}");
                    await RollbackAccountCreationAsync(request.ApplicationId, cancellationToken);
                    break;

                case "ProcessPayment":
                    Console.WriteLine($"Refunding payment for ApplicationId={request.ApplicationId}");
                    await RefundPaymentAsync(request.ApplicationId, cancellationToken);
                    break;

                default:
                    Console.WriteLine($"No compensation action defined for step: {request.FailedStep}");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in SagaCompensationCommandHandler: {ex.Message}");
            throw;
        }
    }


    #region To be defined else where
    private Task RollbackKycHandlingAsync(Guid applicationId, string reason, CancellationToken cancellationToken)
    {
        // Implement rollback logic for KYC handling (e.g., notify user, mark as unverified)
        Console.WriteLine($"Rollback KYC handling: ApplicationId={applicationId}, Reason={reason}");
        return Task.CompletedTask;
    }

    private Task CompensateKycFailureHandlingAsync(Guid applicationId, string reason, CancellationToken cancellationToken)
    {
        // Implement compensation logic for KYC failure (e.g., log failure, escalate to manual review)
        Console.WriteLine($"Compensate KYC failure: ApplicationId={applicationId}, Reason={reason}");
        return Task.CompletedTask;
    }

    private Task RollbackInvestorCreationAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        // Implement rollback logic for investor creation
        Console.WriteLine($"Rollback investor creation for ApplicationId={applicationId}");
        return Task.CompletedTask;
    }

    private Task RollbackAccountCreationAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        // Implement rollback logic for account creation
        Console.WriteLine($"Rollback account creation for ApplicationId={applicationId}");
        return Task.CompletedTask;
    }

    private Task RefundPaymentAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        // Implement refund logic for payment
        Console.WriteLine($"Refund payment for ApplicationId={applicationId}");
        return Task.CompletedTask;
    }
    
    #endregion
}
