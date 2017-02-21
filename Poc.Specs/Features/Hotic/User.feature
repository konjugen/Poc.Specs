@hotic @user
Feature: User
	This is a test for examining user progresses

	@validlogin @smoke_hotic
Scenario: TC-001-Valid user login
	Given I am on the "/login"
	And The following element filled with data:
		| name      | value			           |
		| #Email    | site.admin@inveon.com.tr |
		| #Password | zFh@23e?b		           |
	When I click a ".login-button"
	Then I should see url "/?login=1"

	@validlogin @smoke_hotic
Scenario: TC-002-Invalid user login
	Given I am on the "/login"
	And The following element filled with data:
		| name      | value			    |
		| #Email    | asd@inveon.com.tr |
		| #Password | zFh@23e?b		    |
	When I click a ".login-button"
	Then I should see element ".validation-summary-errors"
