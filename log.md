# Log

## Asummptions
The CreateInvestorRequest object is not fully populated from the User object, and it is assumed that the processor can handle any missing fields.
Error handling has not been implemented in the processor.
Basic validation is absent in the processor.
Logging mechanisms are not integrated into the processor.
All processors and services are assumed to be fully implemented.
Dependency Injection (DI) is assumed to be configured.
Continuous Integration and Continuous Deployment (CI/CD) pipelines are assumed to be in place.
It has been assumed that no rollback is required if a method fails within the Administrator services.

## Decisions
Opted to implement the Factory Pattern to create processors and validators, ensuring the design is easily extensible for future requirements.
Moved product-specific code to validators and processors for improved readability and maintainability, though this could also be handled within the factories.
While it is best practice to wrap third-party calls in try-catch blocks and implement robust logging for easier flow tracking, I refrained from adding error handling as specified in the README. As a result, exceptions like AdministratorException from AdministratorOne are not caught.
Decided to use a base class to streamline processor logic for validation and KYC reporting. While this introduced some additional coupling for derived classes—since the base class has dependencies not required by all derived classes—it aligns with the current requirements. This approach sacrifices some flexibility in validation for individual processors but eliminates code duplication, enhancing readability and maintainability. Should the need arise for different validation or KYC requirements for specific processors in the future, the logic can be easily moved or overridden in the derived classes. This decision meets the specified requirements, which suggest the current logic is unlikely to change in the near future.

## Observations
The Result class has been implemented effectively. It is considered a best practice to use a Result class to encapsulate and return the outcome of an operation, providing a clear structure for handling both success and failure scenarios.

## Todo
Implement endpoints and create e2e tests.

