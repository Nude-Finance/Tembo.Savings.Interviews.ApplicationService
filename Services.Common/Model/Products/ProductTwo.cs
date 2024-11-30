using Services.Common.Abstractions.Abstractions;

namespace Services.Common.Abstractions.Model.Products;

public class ProductTwo : IProduct
{
    public static ProductCode ProductCode => ProductCode.ProductTwo;
}