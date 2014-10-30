Feature: Payment Add
	In order to get the items in the order fulfilled, they need to be paid for.
	As a Customer
	I want to Add a Payment to my Order 
	So that I can take ownership of the products

Background: 
Given I have a unique order number {orderNo}

@addTender
Scenario: Add a cash payment to order 
	When I add a Payment to an order by calling PUT /orders/{orderNo}/payments with a tender type of CASH and an amount of 10.00 GBP
	Then the result should be an HTTP 202 Accepted Status
	And the result should contain a Location header that matches /orders/{orderNo}/requests/.+