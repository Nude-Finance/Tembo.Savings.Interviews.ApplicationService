namespace Services.Common.Abstractions.Validators;

using Model;

public interface IMoneyValidator
{
	bool IsMoneyValid(Money money, Money? minMoney=null,Money? maxMoney=null);
}

public class MoneyValidator : IMoneyValidator
{
	public bool IsMoneyValid(Money money, Money? minMoney = null, Money? maxMoney = null)
	{
		if (minMoney == null &&
		    maxMoney == null)
		{
			throw new ArgumentException("Either minMoney or maxMoney must be specified.");
		}
		
		if (minMoney != null && minMoney.Currency != money.Currency)
		{
			throw new InvalidOperationException($"Currency mismatch: Money currency ({money.Currency}) and minMoney currency ({minMoney.Currency}) must match.");
		}

		if (maxMoney != null && maxMoney.Currency != money.Currency)
		{
			throw new InvalidOperationException($"Currency mismatch: Money currency ({money.Currency}) and maxMoney currency ({maxMoney.Currency}) must match.");
		}

		if (minMoney != null && money.Amount < minMoney.Amount)
		{
			return false;
		}

		if (maxMoney != null && money.Amount > maxMoney.Amount)
		{
			return false;
		}

		return true;
	}
}
