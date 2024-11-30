using Services.Common.Abstractions.Model;

namespace Services.Common.Abstractions.Abstractions;

public interface IProduct
{
    public static abstract ProductCode ProductCode { get; }
}