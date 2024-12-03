using Services.Application.Domain.Interfaces;

namespace Services.Application.Domain;

public class RulesEngine
{
    private readonly IEnumerable<IRule> _rules;

    public RulesEngine(IEnumerable<IRule> rules)
    {
        _rules = rules;
    }

    public bool Validate(Common.Model.Application application, out List<string> errors)
    {
        errors = new List<string>();
        foreach (var rule in _rules)
        {
            if (!rule.IsValid(application))
            {
                errors.Add(rule.ErrorMessage);
            }
        }
        return !errors.Any();
    }
}