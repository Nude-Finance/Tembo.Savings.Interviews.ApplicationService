using Services.Application.Domain.Interfaces;

namespace Services.Application.Domain.Rules;

public class AgeRule : IRule
{
    private readonly int _minAge;
    private readonly int _maxAge;

    public AgeRule(int minAge, int maxAge)
    {
        _minAge = minAge == int.MinValue ? 0 : minAge;
        _maxAge = maxAge == int.MaxValue ? 200 : maxAge;
    }

    public bool IsValid(Common.Model.Application application)
    {
        if(_maxAge < _minAge)
        {
            throw new InvalidOperationException("Max age must be greater than min age.");
        }
        
        var today = DateTime.Today;
        
        var minDate = today.AddYears(-_maxAge); 
        var maxDate = today.AddYears(-_minAge);

        DateTime dob;
        try
        {
            dob = application.Applicant.DateOfBirth.ToDateTime(default);
        }
        catch
        {
            return false;
        }
        
        return dob >= minDate && dob<= maxDate;
    }

    public string ErrorMessage => $"Applicant age must be between {_minAge} and {_maxAge}.";
}