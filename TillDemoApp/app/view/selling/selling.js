'use strict';

angular.module('tillDempApp.selling', ['ngRoute'])
    .config([
        '$routeProvider', function ($routeProvider) {
            $routeProvider.when('/selling', {
                templateUrl: 'view/selling/selling.html',
                controller: 'SellingCtrl'
            });
        }
    ])
    .controller('SellingCtrl', [
        '$scope', '$http', 'productAdd', 'notify', 'productInfo', function ($scope, $http, productAddSvc, notifySvc, productInfo) {
            $scope.order = {};

            if ($scope.orderId == undefined)
                $scope.orderId = "trn:tesco:order:uuid:1b4b0931-5854-489b-a77c-0cebd15d554b";
            autoRefreshOrder();

            function autoRefreshOrder() {
                console.log("refreshing order...");
                getOrder($scope.orderId);
                // Get the Customer Order
                // setTimeout(autoRefreshOrder, 2000);
            }

            function getOrder(orderId) {
                console.log("getting order..." + orderId);
                $http.get('http://localhost:3579/orders/' + orderId + '/')
                    .success(function (order) {
                        $scope.order = order;
                        console.log(order);
                    })
                    .error(function (data) {
                        console.log('Error: ' + data);
                    });
            }

            // when submitting the add form, send the text to the node API
            $scope.addProductByForm = function () {
                productAddSvc.addProduct($scope.orderId, $scope.formData, productAddComplete);
                $scope.formData = {}; // clear the form so our user is ready to enter another
            };

            $scope.addProductById = function (productId) {
                productAddSvc.addProduct($scope.orderId, { productId: productId }, productAddComplete);
            };

            function productAddComplete(request, contentType, data) {
                if (contentType == 'application/vnd.tesco.CustomerOrder.ProductNotFoundException+json') {
                    notifySvc.warn('Not Found', 'Cannot find product {{productId}}', { "productId": request.productId });
                } else if (contentType == 'application/vnd.tesco.CustomerOrder.ProductAdded+json') {
                    productInfo.getProduct(request.productId).then(function (product) {
                        notifySvc.success('Product Added', '{{product}} successfully added.', { "product": product.description });
                    });
                }
                getOrder($scope.orderId);
            };
        }
    ]);