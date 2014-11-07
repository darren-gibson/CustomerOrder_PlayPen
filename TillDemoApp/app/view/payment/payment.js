'use strict';

angular.module('tillDempApp.payment', ['ngRoute'])
    .config([
        '$routeProvider', function ($routeProvider) {
            $routeProvider.when('/payment', {
                templateUrl: 'view/payment/payment.html',
                controller: 'PaymentCtrl'
            });
        }
    ])
    .controller('PaymentCtrl', [
        '$scope', 'order', 'moneyFilter', 'notify', 'payment', function ($scope, orderSvc, moneyFilter, notifySvc, paymentSvc) {
            $scope.order = {};
            autoRefreshOrder();

            function autoRefreshOrder() {
                orderSvc.getOrder().then(function(order) {
                    $scope.order = order;
                });
                // setTimeout(autoRefreshOrder, 2000);
            }

            $scope.startNewOrder = function () {
                orderSvc.new();
                autoRefreshOrder();
            };

            $scope.addPaymentByTenderType = function (tenderType) {
                var amount = $scope.order.total.due;
                paymentSvc.addPayment(orderSvc.getOrderId(), { "tenderType": tenderType, "amount": amount })
                    .then(function (result) {
                        notifySvc.success('Payment Added', '{{tenderType}} payment of {{amount}} successfully added.', { "tenderType": result.tenderType, "amount": moneyFilter(result.data.amount) });
                        autoRefreshOrder();
                    }, function (result) {
                        if (result.contentType == 'application/vnd.tesco.CustomerOrder.InvalidStateException+json') {
                            notifySvc.error('Cannot Add Payment', 'Cannot add a Payment now as the order is not in the correct state');
                        } else {
                            notifySvc.error('Cannot Add Payment', 'Unable to add Payment, unknown error: {{error}}', { "error": result.contentType });
                        }
                        autoRefreshOrder();
                    });
            };
        }
    ]);