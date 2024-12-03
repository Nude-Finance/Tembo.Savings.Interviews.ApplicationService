using Services.Common.Model;

namespace Services.Application.Domain.Interfaces;

public interface IProcessingService
{
    Task Process(ProductCode productCode, Common.Model.Application application);
}