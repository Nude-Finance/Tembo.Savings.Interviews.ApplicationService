using Services.Common.Abstractions.Model;

namespace Services.Common.Abstractions.Abstractions;

public interface IApplicationProcessor<TProduct> where TProduct : struct, IProduct 
{
    Task<Result> Process(Application<TProduct> application);
}