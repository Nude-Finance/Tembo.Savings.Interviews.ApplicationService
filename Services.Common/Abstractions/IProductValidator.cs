namespace Services.Common.Abstractions.Abstractions;

using Model;

public interface IProductValidator
{
	ProductCode SupportedProductCode { get; }
	bool Validate(Application application);
}
