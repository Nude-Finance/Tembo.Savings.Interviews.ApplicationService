using Services.Common.Abstractions.Model;

namespace Services.Applications.Strategies.Abstractions;

public interface IInvestorCreator
{
    Task<Guid> CreateInvestorAsync(Application application);
}
