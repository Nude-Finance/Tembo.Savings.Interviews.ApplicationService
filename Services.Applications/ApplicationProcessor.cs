using MediatR;
using Services.Applications.Commands;
using Services.Common.Abstractions.Abstractions;
using Services.Common.Abstractions.Model;

namespace Services.Applications;

public class ApplicationProcessor(IMediator mediator) : IApplicationProcessor
{
    public async Task ProcessAsync(Application application)
    {
        await mediator.Publish(new HandleKycCommand(application));
    }
}