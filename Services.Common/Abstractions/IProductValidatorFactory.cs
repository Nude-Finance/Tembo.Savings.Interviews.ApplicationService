namespace Services.Common.Abstractions.Abstractions;

using Model;
using Validators;

public interface IProductValidatorFactory
{
	IProductValidator? GetValidator(ProductCode productCode);
}

