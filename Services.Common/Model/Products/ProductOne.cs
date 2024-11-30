using Services.Common.Abstractions.Abstractions;

namespace Services.Common.Abstractions.Model.Products;

public class ProductOne : IProduct
{
    public static ProductCode ProductCode => ProductCode.ProductOne;
}