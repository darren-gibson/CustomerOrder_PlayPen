Feature: All Events for an Order
	In order to make it simple for a Consumers processing a single order, but that are interested in all events that happen on that order. You can subscribe to events on that order an receive them all.
	As a Consumer
	I want to subscribe to events on a order
	So that I am notified about everything that goes on

Background: Starting a new order
	Given the following products are know to the Price Service:
		| Product Id                                                  | Unit Price | Sell by UOM | Friendly Name |
		| trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554c | 1.00 GBP   | Each        | Can of Soup   |
	And I have a unique order number {orderNo}

@events
Scenario: There are initially zero events
	When I GET /orders/{orderNo}/events with an Accept header of application/atom+xml
	Then the result should be an HTTP 200 OK Status
	And the result should contain no events

Scenario: When a Product is added then both a productAdded event and a orderPriced event is raised
	Given I add a Product to a new order by calling POST /orders/{orderNo}/productadd with a ProductID of "trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554c"
	When I GET /orders/{orderNo}/events with an Accept header of application/atom+xml
	Then the result should be an HTTP 200 OK Status
	And the result should contain 2 events of type:
	| Event type                                  |
	| application/vnd.tesco.OrderPricedEvent+xml  |
	| application/vnd.tesco.ProductAddedEvent+xml |