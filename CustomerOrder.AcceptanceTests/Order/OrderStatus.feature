Feature: Order Status
An order transitions through a number of states within its life and can only perform certain actions in certain states.  
For example, you cannot pay before you add products, and once you've started payment, you cannot change the basket.
	As a Customer
	I want to ensure that my order does not allow me to do invalid actions on it
	So that I am confident it will work

Background: Starting a new order
	Given the following products are know to the Price Service:
		| Product Id    | Unit Price | Sell by UOM | Friendly Name                                     |
		| 5000157024671 | 0.68 GBP   | Each        | Heinz Baked Beans In Tomato Sauce 415G            |
		| 5053947861260 | 3.29 GBP   | Each        | Tesco Finest British 6 Lincolnshire Sausages 400G |
	And I have a unique order number {orderNo}

Scenario: Orders with a product in it has a status of Shopping.
	An order with one product in it it has a status of Shopping.

	When I add a Product to the order by calling POST /orders/{orderNo}/productadd with a ProductID of "5053947861260" with a Quantity of "1 Each"
	When I GET /orders/{orderNo} with an accept header of application/json
	Then the result should be an HTTP 200 OK Status
	And the order status should be 'Shopping'


Scenario: Close the order once it is fully paid and fulfilled.
	Once an order has been fully paid then it is marked as closed.

	When I add a Product to the order by calling POST /orders/{orderNo}/productadd with a ProductID of "5053947861260" with a Quantity of "1 Each"
	And I add a Payment to an order by calling PUT /orders/{orderNo}/payments with a tender type of CASH and an amount of 3.29 GBP
	When I GET /orders/{orderNo} with an accept header of application/json
	Then the result should be an HTTP 200 OK Status
	And the order status should be 'Complete'