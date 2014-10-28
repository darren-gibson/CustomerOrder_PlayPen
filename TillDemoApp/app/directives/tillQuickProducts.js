'use strict';

angular.module('tillDempApp')
    .controller('tillQuickProductsCtrl', ['$scope', '$http', function($scope, $http) {
        $scope.quickproducts = {};
        console.log("getting quickproducts...");

        $http.get('http://localhost:3579/v2/products?gtin=urn:epc:id:gtin:00000000000055&gtin=urn:epc:id:gtin:05018374888303&gtin=urn:epc:id:gtin:00296220000000&gtin=urn:epc:id:gtin:00266990000000&gtin=urn:epc:id:gtin:05000157024671&fields=GTIN&fields=description&fields=imageUrl')
            .success(function (quickproducts) {
                $scope.quickproducts = quickproducts;
                console.log(quickproducts);
            })
            .error(function (data) {
                console.log('Error: ' + data);
            });
    }])
    .directive('tillQuickProducts', function () {
        return {
            templateUrl: 'directives/tillQuickProducts.html'
        };
    });