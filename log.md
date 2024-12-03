# Log

## Asummptions
- `ProductOne` and `ProductTwo` have different age requirements and minimum payment amounts.
- `Services.AdministratorOne.Abstractions` and `Services.AdministratorTwo.Abstractions` provide the same functionality but expose different interfaces.
- Only verified users (Kyc'd) should have their applications processed.
- The minimum payment amount for both products is subject to change.
- The `DomainEvents` system is already in place and can be used to capture outputs of the application process.
- No one is over 200 years old.

## Decisions
- Implement the `Process` method in `ApplicationProcessor` to handle applications for both `ProductOne` and `ProductTwo`.
- Apply a rules engine to determine whether an application is successful.
- Use dependency injection to manage instances of `IServiceAdministrator`, `IKycService`, and `IBus`.
- Write behavioral tests to ensure the `Process` method works correctly for both products.
- Mock `IServiceAdministrator`, `IKycService`, and `IBus` in tests.
- The term Abstractions typically implies interfaces or abstract classes, which may not accurately represent the contents of Services.Common.Model. Removing it clarifies that this namespace contains concrete models.

## Observations
- The `Services.AdministratorOne.Abstractions` project targets `net6.0`, while the main project targets `net8.0`.
- The `project.assets.json` file indicates that the project uses `PackageReference` for managing dependencies.
- The `readme.md` provides clear requirements and tips for completing the exercise.
- KycFailed domain event is vage and could be more descriptive as to if its KycFailed to respond successfully or if the user failed Kyc.
- "The exercise shouldn't take too hours. Don't implement:" is a typo and should be "The exercise shouldn't take too many hours. Don't implement:". or "The exercise shouldn't take two hours. Don't implement:". 

## Todo
- add Directory.Build.props and Directory.Build.targets to the solution to enforce consistent settings across projects.
- Update the rules to be configurable and not hardcoded
