Feature: Order
	This is a test for order steps

@order_steps
Scenario Outline: Create order with different conditions
	And The following products were in cart "<products>"
	Then I should see element ".success-msg"
	When I go to url "/Cart"
	Examples: 
	| products        |
	| /p/12053/abella |
	
