namespace Services.Common.Model;

public class Payment(BankAccount bankAccount, Money amount)
{
    public BankAccount BankAccount { get; init; } = bankAccount;
    public Money Amount { get; init; } = amount;
}
