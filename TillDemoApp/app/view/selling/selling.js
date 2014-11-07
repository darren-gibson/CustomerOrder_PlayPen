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
        '$scope', 'order', 'productAdd', 'notify', 'productInfo', function ($scope, orderSvc, productAddSvc, notifySvc, productInfo) {
            $scope.order = {};
            autoRefreshOrder();

            function autoRefreshOrder() {
                orderSvc.getOrder().then(function (order) {
                    $scope.order = order;
                });
                // setTimeout(autoRefreshOrder, 2000);
            }

            // when submitting the add form, send the text to the node API
            $scope.addProductByForm = function () {
                productAddSvc.addProduct(orderSvc.getOrderId(), $scope.formData, productAddComplete);
                $scope.formData = {}; // clear the form so our user is ready to enter another
            };

            $scope.addProductById = function (productId) {
                productAddSvc.addProduct(orderSvc.getOrderId(), { productId: productId }, productAddComplete);
            };

            function productAddComplete(request, contentType, data) {
                if (contentType == 'application/vnd.tesco.CustomerOrder.ProductNotFoundException+json') {
                    notifySvc.warn('Not Found', 'Cannot find product {{productId}}', { "productId": request.productId });
                } else if (contentType == 'application/vnd.tesco.CustomerOrder.ProductAdded+json') {
                    productInfo.getProduct(request.productId).then(function (product) {
                        notifySvc.success('Product Added', '{{product}} successfully added.', { "product": product.description });
                    });
                } else if (contentType == 'application/vnd.tesco.CustomerOrder.InvalidStateException+json') {
                    notifySvc.error('Cannot Add Product', 'Cannot add a Product now as the order is not in the correct state');
                }
                autoRefreshOrder();
            };
        }
    ]);