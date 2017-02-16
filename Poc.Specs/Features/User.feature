Feature: User
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@validlogin @smoke_ciceksepeti
Scenario: Valid user login
	Given I am on the "/login"
	And The following element filled with data:
		| name      | value			           |
		| #Email    | site.admin@inveon.com.tr |
		| #Password | zFh@23e?b		           |
	When I click a ".login-button"
