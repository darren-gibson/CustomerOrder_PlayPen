Feature: Get Order
In order for the customer to pay for the products in their order, they need to see what's in it and the outstanding balance.
	As a Customer
	I want to see my order
	So that I can ensure it is correct

Background: Starting a new order
	Given the following products are know to the Price Service:
		| Product Id    | Unit Price | Sell by UOM | Friendly Name                                     |
		| 5000157024671 | 0.68 GBP   | Each        | Heinz Baked Beans In Tomato Sauce 415G            |
		| 5053947861260 | 3.29 GBP   | Each        | Tesco Finest British 6 Lincolnshire Sausages 400G |
	And I have a unique order number {orderNo}

@addProduct
Scenario: Getting an order with a product in it
	When I add a Product to the order by calling POST /orders/{orderNo}/productadd with a ProductID of "5053947861260"
	When I GET /orders/{orderNo} with an accept header of application/json
	Then the result should be an HTTP 200 OK Status
	And the order should contain the following products:
	| Product                        | Quantity |
	| urn:epc:id:gtin:05053947861260 | 1 Each   |

@addProduct
Scenario: Getting an order with multiple products in it
	When I add a Product to the order by calling POST /orders/{orderNo}/productadd with a ProductID of "5053947861260"
	And I add a Product to the order by calling POST /orders/{orderNo}/productadd with a ProductID of "5000157024671"
	When I GET /orders/{orderNo} with an accept header of application/json
	Then the result should be an HTTP 200 OK Status
	And the order should contain the following products:
	| Product                        | Quantity |
	| urn:epc:id:gtin:05053947861260 | 1 Each   |
	| urn:epc:id:gtin:05000157024671 | 1 Each   |

@addProduct
Scenario: Getting an order with multiple products in it and different quantities
	When I add a Product to the order by calling POST /orders/{orderNo}/productadd with a ProductID of "5053947861260" with a Quantity of "2 Each"
	And I add a Product to the order by calling POST /orders/{orderNo}/productadd with a ProductID of "5000157024671"
	When I GET /orders/{orderNo} with an accept header of application/json
	Then the result should be an HTTP 200 OK Status
	And the order should contain the following products:
	| Product                        | Quantity | Unit Price | Net Price |
	| urn:epc:id:gtin:05053947861260 | 2 Each   | 3.29 GBP   | 6.58 GBP  |
	| urn:epc:id:gtin:05000157024671 | 1 Each   | 0.68 GBP   | 0.68 GBP  |

Scenario: An order is priced with a netTotal amount that tells the customer exactly how much they will need to pay
	When I add a Product to the order by calling POST /orders/{orderNo}/productadd with a ProductID of "5053947861260" with a Quantity of "2 Each"
	And I add a Product to the order by calling POST /orders/{orderNo}/productadd with a ProductID of "5000157024671"
	When I GET /orders/{orderNo} with an accept header of application/json
	Then the result should be an HTTP 200 OK Status
	And the order should have a total.net of 7.26 GBP

Scenario: The amount due for an order is reduced by the total amount paid
	When I add a Product to the order by calling POST /orders/{orderNo}/productadd with a ProductID of "5053947861260" with a Quantity of "3 Each"
	And I add a Product to the order by calling POST /orders/{orderNo}/productadd with a ProductID of "5000157024671"
	And I add a Payment to an order by calling PUT /orders/{orderNo}/payments with a tender type of CASH and an amount of 2.25 GBP
	When I GET /orders/{orderNo} with an accept header of application/json
	Then the result should be an HTTP 200 OK Status
	And the order should have a total.net of 10.55 GBP
	And the order should have a total.paid of 2.25 GBP
	And the order should have a total.due of 8.30 GBP

Scenario: When I get the order the individual payments are available in it
	When I add a Product to the order by calling POST /orders/{orderNo}/productadd with a ProductID of "5053947861260" with a Quantity of "3 Each"
	And I add a Payment to an order by calling PUT /orders/{orderNo}/payments with a tender type of CASH and an amount of 2.25 GBP
	And I add a Payment to an order by calling PUT /orders/{orderNo}/payments with a tender type of VISA and an amount of 4.00 GBP
	When I GET /orders/{orderNo} with an accept header of application/json
	Then the result should be an HTTP 200 OK Status
	And the order should contain the following payments:
	| Tender Type | Amount   |
	| CASH        | 2.25 GBP |
	| VISA        | 4.00 GBP |