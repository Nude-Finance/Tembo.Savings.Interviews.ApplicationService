using Services.Common.Abstractions.Abstractions;

namespace Services.Common.Abstractions.Model.Products;

public struct ProductTwo : IProduct
{
    public ProductCode ProductCode => ProductCode.ProductTwo;
}