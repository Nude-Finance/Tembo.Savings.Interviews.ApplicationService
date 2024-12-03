using Services.Application.Domain.Interfaces;
using Services.Common.Model;

namespace Services.Application.Domain.Rules;

public class PaymentAmountRule : IRule
{
    private readonly Money _minAmount;

    public PaymentAmountRule(Money minAmount)
    {
        _minAmount = minAmount;
    }

    public bool IsValid(Common.Model.Application application)
    {
        return application.Payment.Amount.Amount >= _minAmount.Amount;
    }

    public string ErrorMessage => $"Payment amount must be at least {_minAmount}.";
}