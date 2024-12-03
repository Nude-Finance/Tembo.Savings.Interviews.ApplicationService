namespace Services.Application.Domain.Interfaces;

public interface IRule
{
    bool IsValid(Common.Model.Application application);
    string ErrorMessage { get; }
}