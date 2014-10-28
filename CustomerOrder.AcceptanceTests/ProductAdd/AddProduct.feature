Feature: Add Product
	In order to sell products, they need to be added to the order.
	As a Customer
	I want to Add a Produt to my Order 
	So that I can purchase it

Background: Starting a new order
	Given the following products are know to the Price Service:
		| Product Id                                                  | Unit Price | Sell by UOM | Friendly Name |
		| trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554c | 1.00 GBP   | Each        | Can of Soup   |
	And the system is started
	And I have a unique order number {orderNo}

@addProduct
Scenario: Add a product to an order using the default quantity
	When I add a Product to a new order by calling POST /orders/{orderNo}/productadd with a ProductID of "trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554c"
	Then the result should be an HTTP 202 Accepted Status
	And the result should contain a Location header that matches /orders/{orderNo}/requests/.+

@addProduct @productAdded
Scenario: the result of adding a product to the order is available
	so that I understand if the request to add a product to the order was successful and can find the price of the item that was added.

	Given I add a Product to a new order by calling POST /orders/{orderNo}/productadd with a ProductID of "trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554c" and a Quantity of 3 Each
	When I GET the resource identified by the Uri in the Location Header with an Accept header of application/json
	Then the result should be an HTTP 200 OK Status
	And the result should contain: 
		| Name              | Value                                                       |
		| Content-Type      | application/vnd.tesco.CustomerOrder.ProductAdded+json       |
		| productIdentifier | trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554c |
		| quantity          | 3 Each                                                      |
		| unitPrice         | 1.00 GBP                                                    |
		| netPrice          | 3.00 GBP                                                    |

Scenario: return NotComplete when a ProductAdd request is sent to an order, and the status is checked before the request has completed
	so that the consumer can be informed that the requested is still in progress and has not finished.

	Given I stop the service command runner
	And I add a Product to a new order by calling POST /orders/{orderNo}/productadd with a ProductID of "trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-999999999999" 
	When I GET the resource identified by the Uri in the Location Header with an Accept header of application/json
	Then the result should be an HTTP 200 OK Status
	And the result should contain: 
		| Name         | Value                                                |
		| Content-Type | application/vnd.tesco.CustomerOrder.NotComplete+json |

@addProduct @productNotFoundException @exception
Scenario: return a ProductNotFoundException when an attempt is made to add a product that is not known to the price service
	so that the consumer can be informed that the requested product was not added to the customer order.

	Given I add a Product to a new order by calling POST /orders/{orderNo}/productadd with a ProductID of "trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-999999999999"
	When I GET the resource identified by the Uri in the Location Header with an Accept header of application/json
	Then the result should be an HTTP 200 OK Status
	And the result should contain: 
		| Name         | Value                                                             |
		| Content-Type | application/vnd.tesco.CustomerOrder.ProductNotFoundException+json |