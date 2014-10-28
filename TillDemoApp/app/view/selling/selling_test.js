'use strict';

describe('tillDempApp.selling module', function () {

    beforeEach(module('tillDempApp.selling'));

    describe('selling controller', function () {

        var scope ,controller, httpBackend, http;
        beforeEach(inject(function($rootScope, $controller, $httpBackend, $http) {
            scope = $rootScope.$new();
            scope.orderId = "123";
            httpBackend = $httpBackend;
            http = $http;
            httpBackend.when("GET", "http://localhost:3579/orders/123/").respond(
                { "products": [{ "productId": "trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554b", "quantity": { "amount": 2.0, "uom": "Each" }, "price": { "unit": { "amount": 0.50, "currency": "GBP" }, "net": { "amount": 1.00, "currency": "GBP" } } }, { "productId": "trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554b", "quantity": { "amount": 2.0, "uom": "Each" }, "price": { "unit": { "amount": 0.50, "currency": "GBP" }, "net": { "amount": 1.00, "currency": "GBP" } } }], "total": { "net": { "amount": 2.00, "currency": "GBP" } } });
            controller = $controller;

            // Mock the setTimeout to prevent the automatic callback
            spyOn(window, 'setTimeout');
        }));

        it('should be defined', function () {
            var sellingController = controller('SellingCtrl', { $scope: scope, $http: http });
            expect(sellingController).toBeDefined();
        });

        it('should store the order in scope', function () {
            controller('SellingCtrl', { $scope: scope, $http: http });
            expect(scope.order).toBeDefined();
        });

        it('should call the server to get the latest order', function () {
            controller('SellingCtrl', { $scope: scope, $http: http });
            httpBackend.expectGET('http://localhost:3579/orders/123/');
            httpBackend.flush();
        });

        it('should use the orderId from scope to call the server to get the latest order', function () {
            httpBackend.when("GET", "http://localhost:3579/orders/999/").respond({ "products": [], "total": { "net": { "amount": 0.00, "currency": "GBP" } } });
            scope.orderId = "999";
            controller('SellingCtrl', { $scope: scope, $http: http });
            httpBackend.expectGET('http://localhost:3579/orders/999/');
            httpBackend.flush();
        });

        it('should use have called the setTimeout function to call the getOrderEvery 2 seconds', function () {
            var c= controller('SellingCtrl', { $scope: scope, $http: http });
            expect(window.setTimeout).toHaveBeenCalled();
        });
    });
});