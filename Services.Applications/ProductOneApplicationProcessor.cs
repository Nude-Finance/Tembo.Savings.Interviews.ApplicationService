using Services.Common.Abstractions.Abstractions;
using Services.Common.Abstractions.Model;
using Services.Common.Abstractions.Model.Products;

namespace Services.Applications;

public class ProductOneApplicationProcessor : IApplicationProcessor<ProductOne>
{
    public Task Process(Application<ProductOne> application)
    {
        throw new NotImplementedException();
    }
}