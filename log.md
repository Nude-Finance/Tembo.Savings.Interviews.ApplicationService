# Log

## Assumptions
1. On the `Application` model, the `User` and `Payment` properties should not be nullable.

## Decisions
1. Create separate Application Processors for each product type.

## Observations
1. It seems that the `ProductCode` enum is only used to interface with AdministratorOne service but it is declared
   in the shared library. If it is possible, we should remove the enum from the shared library and interface 
   with the administrator using the underlying type that represents ProductOne. Let's not let
   3rd-party types bleed into our codebase.

## Todo
1. Create validator test
2. Create validator
3. Implement validation requirements
