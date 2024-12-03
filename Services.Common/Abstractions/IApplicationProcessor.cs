using Services.Common.Model;

namespace Services.Common.Abstractions;

public interface IApplicationProcessor  
{
    Task Process(Application application);
}