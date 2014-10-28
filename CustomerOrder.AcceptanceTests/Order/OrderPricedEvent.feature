Feature: Order Priced Event
In order for the customer to pay for the products in their order, a customer needs to be informed of the total price for the order.
	As a Customer
	I want to know the total amount that the Order will costs 
	So that I can make a decision of whether I want to purchase it

Background: Starting a new order
	Given the following products are know to the Price Service:
		| Product Id    | Unit Price | Sell by UOM | Friendly Name                                     |
		| 5000157024671 | 0.68 GBP   | Each        | Heinz Baked Beans In Tomato Sauce 415G            |
		| 5053947861260 | 3.29 GBP   | Each        | Tesco Finest British 6 Lincolnshire Sausages 400G |

	And I have a unique order number {orderNo}

@addProduct
Scenario: Adding a product to the order causes an OrderPriceEvent to be generated
	When I add a Product to a new order by calling POST /orders/{orderNo}/productadd with a ProductID of "5053947861260"
	Then the result should be an HTTP 202 Accepted Status
	When I GET /orders/{orderNo}/events/orderPriced with an Accept header of application/atom+xml
	Then the result should be an HTTP 200 OK Status
	And the result should contain:
	| Name     | Value     |
	| order    | {orderNo} |
	| netTotal | 3.29 GBP  |