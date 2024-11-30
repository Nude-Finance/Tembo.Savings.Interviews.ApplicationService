using Services.Common.Abstractions.Model;

namespace Services.Common.Abstractions.Abstractions;

public interface IApplicationProcessor<TProduct> where TProduct : class, IProduct 
{
    Task Process(Application<TProduct> application);
}