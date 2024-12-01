# Log

## Assumptions
1. On the `Application` model, the `User` and `Payment` properties should not be nullable.

## Decisions
1. Made Application explicitly specify which product using generics.
3. Created separate Application Processors for each product type.
3. Created separate Application Validators for each product type.

## Observations
1. It seems that the `ProductCode` enum is only used to interface with AdministratorTwo service but it is declared
   in the shared library. If it is possible, we should remove the enum from the shared library and interface 
   with the administrator using the underlying type that represents ProductOne. Let's not let
   3rd-party types bleed into our codebase.

2. AdministratorOne initial payment amount is whole numbers.


## Todo

1. Resolve field discrepancies between `ProductOneApplicationProcessor` / `AdministratorOne`:
   - What goes in Initial Payment (int)?
   - Multiple Addresses - OK to use first?
   - String format to use for DOB
   - Reference - Ok to use ApplicationId?
   - Email - Can be null?
   - Account Number - Bank Account Number?

