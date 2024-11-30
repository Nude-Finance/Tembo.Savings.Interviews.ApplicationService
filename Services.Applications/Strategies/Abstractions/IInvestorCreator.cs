using Services.Common.Abstractions.Model;

namespace Services.Applications.Abstractions;

public interface IInvestorCreator
{
    Task<string> CreateInvestorAsync(Application application);
}
