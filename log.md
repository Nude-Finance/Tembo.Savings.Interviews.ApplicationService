# Log

## Assumptions
1. AdministratorOne --> Does the entire workflow in a single operation --> CreateInvestor
2. AdministratorTwo --> Multiple operations to be carried out in a transactional workflow to complete the process
3. AdministratorOne and Two can be considered as a sort of stranglers. AdministratorTwo strangling as the new workflow.
4. We already have an error handling and exception handling in place.
5. Can stick on to the ILogger implementation to allow any logging frameworks to be used.

## Decisions
1. Define strategies to select between ProductOne and ProductTwo.
2. Select the strategy in the context based on the product code.
3. For both products start the process flow with KYC validation and handling.
4. Always do the KYC check rather than relying on the property to verify it.
5. API's to be exposed for downstreams to fetch the account, investor and payment details. This is because we will be only sending the ApplicationId and InvestorId.
6. Again, sticking on to ApplicationId and InvestorId due to differences in the response of AdministratorOne and AdministratorTwo.
7. Leave the DLQ's to down streams or decide based on the agreement, who is going to handle what.
 
## Observations
1. Mismatch in data type of ids.
2. Both the administrators does the same thing but in different workflow steps.

## Todo

1. Foreach saga, handle the repo CRUDs.
2. Handle Rollback scenarios.
3. While calling the administrators, implement the CircuitBreakers, retry and exponential backoff. While doing this, make sure to update it back to the sagaCompensationHandler.
