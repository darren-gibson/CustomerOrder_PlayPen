'use strict';

angular.module('tillDempApp').factory('payment', ['$http', '$log', '$q', function ($http, $log, $q) {
    var service = {};

    function makeResult(contentType, data) {
        return { contentType: contentType, data: data };
    };


    service.addPayment = function (orderId, paymentToAdd) {
        var deferred = $q.defer();

        $http.put('http://localhost:3579/orders/' + orderId + '/payments/', paymentToAdd)
            .success(function (data, status, headers) {
                $log.info(data);
                var commandResultUrl = headers("Location");
                service.waitForPaymentAddToComplete(paymentToAdd, commandResultUrl, deferred);
            })
            .error(function (data, status, headers) {
                $log.error('Error: ' + data);
                deferred.reject(makeResult(headers("Content-Type"), data));
            });
        return deferred.promise;
    };

    service.waitForPaymentAddToComplete = function (paymentToAdd, commandResultUrl, deferred) {
        $http.get(commandResultUrl)
            .success(function (commandResult, status, headers) {
                $log.info("waitForPaymentAddToComplete Result=" + headers("Content-Type") + ", Data=" + commandResult);
                if (service.isStillRunning(commandResult, headers)) {
                    setTimeout(function () { service.waitForPaymentAddToComplete(paymentToAdd, commandResultUrl, deferred); }, 500);
                } else {
                    var contentType = headers("Content-Type");

                    if (contentType == 'application/vnd.tesco.CustomerOrder.PaymentAdded+json') {
                        deferred.resolve(makeResult(contentType, commandResult));
                    } else {
                        deferred.reject(makeResult(contentType, commandResult));
                    }
                }
            })
            .error(function (commandResult, status, headers) {
                $log.error("waitForPaymentAddToComplete, status=" + status + ", Result=" + headers("Content-Type"));
                deferred.reject(makeResult(headers("Content-Type"), data));
            });
    };

    service.isStillRunning = function (commandResult, headers) {
        return headers("Content-Type") == "application/vnd.tesco.CustomerOrder.NotComplete+json";
    };

    return service;
}
]);