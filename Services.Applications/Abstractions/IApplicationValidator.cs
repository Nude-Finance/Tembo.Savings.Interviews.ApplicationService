using Services.Common.Abstractions.Abstractions;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Abstractions;

public interface IApplicationValidator<TProduct> where TProduct : struct, IProduct
{
    public Result Validate(Application<TProduct> productApplication);
}