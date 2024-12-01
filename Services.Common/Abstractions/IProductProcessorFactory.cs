namespace Services.Common.Abstractions.Abstractions;

using Model;

public interface IProductProcessorFactory
{
	IProductProcessor GetProcessor(ProductCode productCode);
}
