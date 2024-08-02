Feature: UsersPlanProcedure



@AddUsersToPlan
Scenario: AssignUsersToPlanProcedire
	Given I'm on the start page
	When I click on start
	Then I'm on the plan page
	
	When I Select Procedures To Plan
	Then I See Plan Procedure
	Then I Assign Users To Plan Procedure
	
	When I Refresh 
	Then The Selected Users Should Be Retained


