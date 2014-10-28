'use strict';

angular.module('tillDempApp').factory('productAdd', [
    '$http', '$log', function ($http, $log) {
        var service = {};

        service.addProduct = function (orderId, productToAdd, onSuccess, onError) {
            $http.post('http://localhost:3579/orders/' + orderId + '/productadd', productToAdd)
                .success(function (data, status, headers) {
                    $log.info(data);
                    var commandResultUrl = headers("Location");
                    service.waitForProductAddToComplete(productToAdd, commandResultUrl, onSuccess, onError);
                })
                .error(function (data) {
                    $log.error('Error: ' + data);
                });
        };

        service.waitForProductAddToComplete = function (productToAdd, commandResultUrl, onSuccess, onError) {
            $http.get(commandResultUrl)
                .success(function (commandResult, status, headers) {
                    $log.info("waitForProductAddToComplete Result=" + headers("Content-Type") + ", Data=" + commandResult);
                    if (service.isStillRunning(commandResult, headers)) {
                        setTimeout(function () { service.waitForProductAddToComplete(productToAdd, commandResultUrl, onSuccess, onError); }, 500);
                    } else {
                        if (onSuccess != null)
                            onSuccess(productToAdd, headers("Content-Type"), commandResult);
                    }
                })
                .error(function (commandResult, status, headers) {
                    $log.error("waitForProductAddToComplete, status=" + status + ", data=" + commandResult);
                    if (onError != null)
                        onError(productToAdd, headers("Content-Type"), commandResult);
                });
        };

        service.isStillRunning = function (commandResult, headers) {
            return headers("Content-Type") == "application/vnd.tesco.CustomerOrder.NotComplete+json";
        };

        return service;
    }
]);