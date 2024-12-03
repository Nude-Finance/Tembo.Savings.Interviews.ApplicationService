using Services.Common.Model;

namespace Services.Application.Domain.Interfaces;

public interface IAdministratorFactory
{
    (IServiceAdministrator, IEnumerable<IRule>) GetAdministratorAndRules(ProductCode productCode);
}