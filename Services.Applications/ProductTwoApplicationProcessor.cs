using Services.Common.Abstractions.Abstractions;
using Services.Common.Abstractions.Model;
using Services.Common.Abstractions.Model.Products;

namespace Services.Applications;

public class ProductTwoApplicationProcessor : IApplicationProcessor<ProductTwo>
{
    public Task Process(Application<ProductTwo> application)
    {
        throw new NotImplementedException();
    }
}