Feature: Payment Add
	In order to get the items in the order fulfilled, they need to be paid for.
	As a Customer
	I want to Add a Payment to my Order 
	So that I can take ownership of the products

Background: 
Given I have a unique order number {orderNo}

@addPayment
Scenario: Add a cash payment to order 
	When I add a Payment to an order by calling PUT /orders/{orderNo}/payments with a tender type of CASH and an amount of 10.00 GBP
	Then the result should be an HTTP 202 Accepted Status
	And the result should contain a Location header that matches /orders/{orderNo}/requests/.+

@addPayment @paymentAdded
Scenario: the result of calling payment adding a payment to the order should 
	so that the success status of adding the payment to the order is becomes available.

	When I add a Payment to an order by calling PUT /orders/{orderNo}/payments with a tender type of CASH and an amount of 10.00 GBP
	And I GET the resource identified by the Uri in the Location Header with an Accept header of application/json
	Then the result should be an HTTP 200 OK Status
	And the result should contain: 
		| Name         | Value                                                 |
		| Content-Type | application/vnd.tesco.CustomerOrder.PaymentAdded+json |
		| tenderType   | CASH                                                  |
		| amount       | 10.00 GBP                                             |