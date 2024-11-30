# Log

## Asummptions
The fields in CreateInvestorRequest are not fully satisfied from User object so it is assumed that processor can deal with the missing fields.
No error handling is implemented in the processor.
No basic validation is implemented in the processor.
No logging is implemented in the processor.
All processor and services are assumed to be implemented
The DI is assumed to be implemented
CI CD is assumed to be implemented

## Decisions
Decided to implement factory pattern to create the processor and validators, so that it can be easily extended in future.
Put product code to validators and processor to make it more readable and maintainable, but this can be done in the factories as well.
Decided to add a new event to handle validation fails and decided to publish this event in the application processor when process returns fail.


## Observations
The result claas is implemented nicely and it is a good practice to have a result class to return the result of the operation.

## Todo
Implement endpoints and create e2e tests.

