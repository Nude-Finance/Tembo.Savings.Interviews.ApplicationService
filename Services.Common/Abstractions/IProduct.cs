using Services.Common.Abstractions.Model;

namespace Services.Common.Abstractions.Abstractions;

public interface IProduct
{
    public ProductCode ProductCode { get; }
}