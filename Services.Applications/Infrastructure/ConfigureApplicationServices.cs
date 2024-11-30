using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Services.Applications.Commands;
using Services.Applications.Commands.Handlers;
using Services.Applications.Strategies.Context;
using Services.Applications.Strategies.InvestorStrategy;

namespace Services.Applications.Infrastructure;

public static class RegisterDI
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ProductOneInvestorCreator>();
        services.AddScoped<ProductTwoInvestorCreator>();
        services.AddScoped<InvestorContext>();
        services.AddScoped<IRequestHandler<CreateInvestorCommand>, CreateInvestorCommandHandler>();
        return services;
    }
}