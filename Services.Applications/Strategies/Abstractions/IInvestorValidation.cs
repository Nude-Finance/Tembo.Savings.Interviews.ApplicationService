using Services.Common.Abstractions.Model;

namespace Services.Applications.Strategies.Abstractions;

public interface IInvestorValidation
{
    void Validate(Application application);
}
