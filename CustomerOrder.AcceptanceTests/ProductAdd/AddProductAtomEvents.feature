Feature: Add Product Atom Events
	In order to sell products, they need to be added to the order.
	As a Customer
	I want to Add a Produt to my Order 
	So that I can purchase it

Background: Starting a new order
	Given the following products are know to the Price Service:
		| Product Id                                                  | Unit Price | Sell by UOM | Friendly Name |
		| trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554c | 1.00 GBP   | Each        | Can of Soup   |
	And I have a unique order number {orderNo}

@addProduct @productAdded
Scenario: There are initially zero events
	When I GET /orders/{orderNo}/events/productAdded with an Accept header of application/atom+xml
	Then the result should be an HTTP 200 OK Status
	And the result should contain no events

@addProduct @productAdded
Scenario: Successfully adding a product to an order generates a productAdded event
	Given I add a Product to a new order by calling POST /orders/{orderNo}/productadd with a ProductID of "trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554c"
	When I GET /orders/{orderNo}/events/productAdded with an Accept header of application/atom+xml
	Then the result should be an HTTP 200 OK Status
	And the result should contain:
	| Name     | Value                                                       |
	| product  | trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554c |
	| order    | {orderNo}                                                   |
	| quantity | 1 Each                                                      |

@addProduct @productAdded @unitPrice @netPrice
Scenario: productAdded events include a unit price and net price
	Given I add a Product to a new order by calling POST /orders/{orderNo}/productadd with a ProductID of "trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554c"
	When I GET /orders/{orderNo}/events/productAdded with an Accept header of application/atom+xml
	Then the result should be an HTTP 200 OK Status
	And the result should contain:
	| Name      | Value    |
	| unitPrice | 1.00 GBP |
	| netPrice  | 1.00 GBP |

@addProduct @productAdded
Scenario: adding a product with a quantity of 3 changes the quantity and the Net Price
	Given I add a Product to a new order by calling POST /orders/{orderNo}/productadd with a ProductID of "trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554c" and a Quantity of 3 Each
	When I GET /orders/{orderNo}/events/productAdded with an Accept header of application/atom+xml
	Then the result should be an HTTP 200 OK Status
	And the result should contain:
	| Name      | Value    |
	| unitPrice | 1.00 GBP |
	| netPrice  | 3.00 GBP |

@addProduct @productAdded @eventId
Scenario: the Id in the productAdded event is stable
	Given I add a Product to a new order by calling POST /orders/{orderNo}/productadd with a ProductID of "trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554c"
	When I GET /orders/{orderNo}/events/productAdded with an Accept header of application/atom+xml
	And I save the Id of the productAdded event
	And  I GET /orders/{orderNo}/events/productAdded with an Accept header of application/atom+xml
	Then the saved Id should equal the Id in the productAdded event just received
	