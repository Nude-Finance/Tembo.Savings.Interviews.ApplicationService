namespace Services.Applications;

using Common.Abstractions.Abstractions;
using Common.Abstractions.Model;
using System.Collections.Concurrent;

public class ProductProcessorFactory : IProductProcessorFactory
{
	private readonly ConcurrentDictionary<ProductCode, IProductProcessor> _processorMap;

	public ProductProcessorFactory(IEnumerable<IProductProcessor> processors)
	{
		_processorMap = new ConcurrentDictionary<ProductCode, IProductProcessor>(
			processors
				.SelectMany(p => p.SupportedProductCodes.Select(code => new { code, processor = p }))
				.GroupBy(x => x.code)
				.ToDictionary(
					g => g.Key,
					g => g.SingleOrDefault()?.processor 
					     ?? throw new InvalidOperationException($"Duplicate processors detected for ProductCode '{g.Key}'. Ensure each ProductCode is supported by only one processor.")
				)
		);

	}

	public IProductProcessor? GetProcessor(ProductCode productCode)
	{
		_processorMap.TryGetValue(productCode, out var processor);
		return processor;
	}
}
