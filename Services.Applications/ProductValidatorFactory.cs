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
			validators
				.GroupBy(validator => validator.SupportedProductCode)
				.ToDictionary(
					g => g.Key,
					g =>
					{
						if (g.Count() > 1) 
						{
							throw new InvalidOperationException($"Duplicate validators detected for ProductCode '{g.Key}'. Ensure each ProductCode is supported by only one validator.");
						}
						return g.Single();
					}
				)!
		);

	}

	public IProductValidator? GetValidator(ProductCode productCode)
	{
		return _validatorMap.GetValueOrDefault(productCode);
	}
}
