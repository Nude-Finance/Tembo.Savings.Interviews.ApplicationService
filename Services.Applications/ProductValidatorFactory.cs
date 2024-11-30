namespace Services.Applications;

using Common.Abstractions.Abstractions;
using Common.Abstractions.Model;
using System.Collections.Concurrent;

public class ProductValidatorFactory : IProductValidatorFactory
{
	private readonly ConcurrentDictionary<ProductCode, IProductValidator> _validatorMap;

	public ProductValidatorFactory(IEnumerable<IProductValidator> validators)
	{
		_validatorMap = new ConcurrentDictionary<ProductCode, IProductValidator>(
			validators.ToDictionary(
				validator => validator.SupportedProductCode,
				validator => validator
			)
		);
	}

	public IProductValidator? GetValidator(ProductCode productCode)
	{
		return _validatorMap.GetValueOrDefault(productCode);
	}
}
