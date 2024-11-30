using Services.Common.Abstractions.Abstractions;

namespace Services.Common.Abstractions.Model.Products;

public struct ProductOne : IProduct
{
    public ProductCode ProductCode => ProductCode.ProductOne;
}