using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Services.Applications.Commands;
using Services.Applications.Commands.Handlers;
using Services.Applications.Strategies.Context;
using Services.Applications.Strategies.InvestorStrategy;

namespace Services.Applications.Infrastructure;

// TO DO: check if required.
public static class ConfigureApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ProductOneInvestorCreator>();
        services.AddScoped<ProductTwoInvestorCreator>();
        services.AddScoped<InvestorContext>();
        services.AddScoped<IRequestHandler<HandleKycCommand>, HandleKycCommandHandler>();
        services.AddScoped<IRequestHandler<CreateInvestorCommand>, CreateInvestorCommandHandler>();
        services.AddScoped<IRequestHandler<CreateAccountCommand>, CreateAccountCommandHandler>();
        services.AddScoped<IRequestHandler<ProcessPaymentCommand>, ProcessPaymentCommandHandler>();
        services.AddScoped<IRequestHandler<SagaCompensationCommand>, SagaCompensationCommandHandler>();

        return services;
    }
}