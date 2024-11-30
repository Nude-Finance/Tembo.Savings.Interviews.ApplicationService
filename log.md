# Log

## Assumptions
1. On the `Application` model, the `User` and `Payment` properties should not be nullable.
2. Contract for AdministratorTwo IAdministrationService should not be broken, 
   i.e. keep ProductCode enum in parameter list. 

## Decisions
1. Create a separate Application Processor for each product type

## Observations

## Todo
1. Make test pass
2. Check test assertion against requirements
3. Create validator test
4. Create validator
5. Implement validation requirements
