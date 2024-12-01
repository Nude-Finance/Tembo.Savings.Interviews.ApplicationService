using Services.Applications.Abstractions;
using Services.Applications.Validation;
using Services.Applications.ValidationHelpers;
using Services.Common.Abstractions.Model;
using Services.Common.Abstractions.Model.Products;

namespace Services.Applications.ApplicationValidators;

public class ProductOneApplicationValidator : IApplicationValidator<ProductOne>
{
    public Result Validate(Application<ProductOne> productApplication)
    {
        var validationResult = AgeValidation.MinAge(Constants.ProductOne.MinAgeYears, productApplication.Applicant.DateOfBirth);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        validationResult = AgeValidation.MaxAge(Constants.ProductOne.MaxAgeYears, productApplication.Applicant.DateOfBirth);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        if (productApplication.Payment.Amount.Amount < Constants.ProductOne.MinDeposit)
        {
            return Result.Failure(new Error("Validation Error", "422", 
                $"Must have minimum payment of {Constants.ProductOne.MinDeposit:0.00}."));
        }
        
        return Result.Success();
    }
}