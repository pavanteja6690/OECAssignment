Feature: Plan feature
    Test creating and adding procedures to a plan

    # example
    Scenario: Create Plan
        Given I'm on the start page
        When I click on start
        Then I'm on the plan page
        When I add a procedure to the plan
        And Assign user to plan procedure
        And refresh the page
        Then verify the user attached to plan procedure


# Expected test
# Scenario: Test Adding user to a plan procedure