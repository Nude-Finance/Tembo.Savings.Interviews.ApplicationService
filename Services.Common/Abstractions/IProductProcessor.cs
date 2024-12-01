namespace Services.Common.Abstractions.Abstractions;

using Model;

public interface IProductProcessor
{
	HashSet<ProductCode> SupportedProductCodes { get; }
	Task ProcessAsync(Application application);
}
