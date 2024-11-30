using Services.Common.Abstractions.Abstractions;

namespace Services.Common.Abstractions.Model;

public class Application<TProduct> where TProduct : class, IProduct {
    public Guid Id { get; init; }
    public required User Applicant { get; init; }
    public required Payment Payment { get; init; }
    public required TProduct Product { get; init; }
}
