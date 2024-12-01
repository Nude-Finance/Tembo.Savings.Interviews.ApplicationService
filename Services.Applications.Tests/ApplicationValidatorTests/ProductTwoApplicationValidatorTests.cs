using AutoFixture;
using Services.Applications.Abstractions;
using Services.Applications.ApplicationValidators;
using Services.Applications.Validation;
using Services.Common.Abstractions.Model;
using Services.Common.Abstractions.Model.Products;
using Shouldly;
using Xunit;

namespace Services.Applications.Tests.ApplicationValidatorTests;

public class ProductTwoApplicationValidatorTests
{
    private readonly Fixture _fixture = new ();
    
    private readonly IApplicationValidator<ProductTwo> _sut = new ProductTwoApplicationValidator();
    
    [Fact]
    public void Given_ApplicantAgeLessThanMinAge_ShouldReturnInvalid()
    {
        //Arrange
        var today = DateTime.Now.Date;
        var dob = new DateOnly(today.Year - Constants.ProductTwo.MinAgeYears, today.Month, today.Day)
            .AddDays(1);
        
        var user = _fixture.Build<User>()
            .With(x => x.DateOfBirth, dob)
            .Create();
        
        var application = _fixture.Build<Application<ProductTwo>>()
            .With(x => x.Applicant, user)
            .Create();
        
        //Act
        var result = _sut.Validate(application);
        
        //Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("422");
        result.Error.System.ShouldBe("Validation Error");
        result.Error.Description.ShouldBe("Must be 18 years or older.");
    }
    
    [Fact]
    public void Given_Payment_LessThanMinPayment_ShouldReturnInvalid()
    { 
        //Arrange
        var today = DateTime.Now.Date;
        var dob = new DateOnly(today.Year - Constants.ProductTwo.MinAgeYears, today.Month, today.Day)
            .AddYears(-1);

        var user = _fixture.Build<User>()
            .With(x => x.DateOfBirth, dob)
            .Create();

        var payment = _fixture.Build<Payment>()
            .With(x => x.Amount, new Money("GBP", Constants.ProductTwo.MinDeposit - 0.01m))
            .Create();
        
        var application = _fixture.Build<Application<ProductTwo>>()
            .With(x => x.Applicant, user)
            .With(x => x.Payment, payment)
            .Create();
        
        //Act
        var result = _sut.Validate(application);
        
        //Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("422");
        result.Error.System.ShouldBe("Validation Error");
        result.Error.Description.ShouldBe("Must have minimum payment of 0.99.");
    }
    
    [Fact]
    public void Given_ValidApplication_ShouldReturnValid()
    {
        //Arrange
        var today = DateTime.Now.Date;
        var dob = new DateOnly(today.Year - Constants.ProductTwo.MinAgeYears, today.Month, today.Day)
            .AddMonths(-1);

        var user = _fixture.Build<User>()
            .With(x => x.DateOfBirth, dob)
            .Create();

        var payment = _fixture.Build<Payment>()
            .With(x => x.Amount, new Money("GBP", Constants.ProductOne.MinDeposit))
            .Create();
        
        var application = _fixture.Build<Application<ProductTwo>>()
            .With(x => x.Applicant, user)
            .With(x => x.Payment, payment)
            .Create();
        
        //Act
        var result = _sut.Validate(application);
        
        //Assert
        result.IsSuccess.ShouldBeTrue();
    }
}