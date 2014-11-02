Feature: Payment Add
	In order to get the items in the order fulfilled, they need to be paid for.
	As a Customer
	I want to Add a Payment to my Order 
	So that I can take ownership of the products

Background: Starting a new order
	Given the following products are know to the Price Service:
		| Product Id    | Unit Price | Sell by UOM | Friendly Name                                     |
		| 5000157024671 | 0.68 GBP   | Each        | Heinz Baked Beans In Tomato Sauce 415G            |
		| 5053947861260 | 3.29 GBP   | Each        | Tesco Finest British 6 Lincolnshire Sausages 400G |
	And I have a unique order number {orderNo}
	And I add a Product to the order by calling POST /orders/{orderNo}/productadd with a ProductID of "5053947861260" with a Quantity of "3 Each"

@addPayment
Scenario: Add a cash payment to order 
	When I add a Payment to an order by calling PUT /orders/{orderNo}/payments with a tender type of CASH and an amount of 10.00 GBP
	Then the result should be an HTTP 202 Accepted Status
	And the result should contain a Location header that matches /orders/{orderNo}/requests/.+

@addPayment @paymentAdded
Scenario: the result of calling payment adding a payment to the order should 
	so that the success status of adding the payment to the order is becomes available.

	When I add a Payment to an order by calling PUT /orders/{orderNo}/payments with a tender type of CASH and an amount of 5.00 GBP
	And I GET the resource identified by the Uri in the Location Header with an Accept header of application/json
	Then the result should be an HTTP 200 OK Status
	And the result should have a Content-Type of 'application/vnd.tesco.CustomerOrder.PaymentAdded+json'
	And the result should contain: 
		| Name         | Value                                                 |
		| tenderType   | CASH                                                  |
		| amount       | 5.00 GBP                                              |

@addPayment
Scenario: Cannot pay more than the value of the order.  
	The payment service is responsible for giving change to the customer and therefore the order is only ever paid upto the amount due (remaining balance)
	When I add a Payment to an order by calling PUT /orders/{orderNo}/payments with a tender type of CASH and an amount of 10.00 GBP
	And I GET the resource identified by the Uri in the Location Header with an Accept header of application/json
	Then the result should be an HTTP 200 OK Status
	And the result should have a Content-Type of 'application/vnd.tesco.CustomerOrder.PaymentExceededAmountDueException+json'
	And the PaymentExceededAmountDueException should contain:
		| Name      | Value    |
		| amountDue | 9.87 GBP |
