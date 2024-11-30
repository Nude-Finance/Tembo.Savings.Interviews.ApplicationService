using Services.Common.Abstractions.Abstractions;
using Services.Common.Abstractions.Model;

namespace Services.Applications;

public class ApplicationProcessor(IProductProcessorFactory productProcessorFactory) : IApplicationProcessor
{
	public async Task Process(Application application)
	{
		var productProcessor = productProcessorFactory.GetProcessor(application.ProductCode);

		await productProcessor.ProcessAsync(application);
	}
}
