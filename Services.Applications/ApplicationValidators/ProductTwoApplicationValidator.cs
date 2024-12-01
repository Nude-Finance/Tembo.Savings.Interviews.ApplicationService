using Services.Applications.Abstractions;
using Services.Applications.Validation;
using Services.Applications.ValidationHelpers;
using Services.Common.Abstractions.Model;
using Services.Common.Abstractions.Model.Products;

namespace Services.Applications.ApplicationValidators;

public class ProductTwoApplicationValidator : IApplicationValidator<ProductTwo>
{
    public Result Validate(Application<ProductTwo> productApplication)
    {
        var validationResult = AgeValidation.MinAge(Constants.ProductTwo.MinAgeYears, productApplication.Applicant.DateOfBirth);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        if (productApplication.Payment.Amount.Amount < Constants.ProductTwo.MinDeposit)
        {
            return Result.Failure(new Error("Validation Error", "422", 
                $"Must have minimum payment of {Constants.ProductOne.MinDeposit:0.00}."));
        }
        
        return Result.Success();
    }
}