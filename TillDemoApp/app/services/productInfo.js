angular.module('tillDempApp').factory('productInfo', ['$http', '$q', '$log', function ($http, $q) {
    var service = {};
    var cache = [];

    service.getProduct = function (id) {
        // perform some asynchronous operation, resolve or reject the promise when appropriate.
        var deferred = $q.defer();

        if (id in cache && cache[id].isFake == false) {
            setTimeout(function () { deferred.resolve(cache[id]); }, 0);
        } else {
            $http.get('http://localhost:3579/v2/products?fields=GTIN&fields=description&fields=imageUrl&gtin=' + id)
                .success(function (result) {
                    cache[id] = result.products.length > 0 ? result.products[0] : { description: "[not found]" };
                    deferred.resolve(cache[id]);
                })
                .error(function (data) {
                    $log.error('cacheProduct Error: ' + data);
                    cache[id] = { description: "[error getting description]" };
                    deferred.resolve(cache[id]);
                });
        }
        return deferred.promise;
    }

    service.getDescription = function (id) {
        if (!(id in cache)) {
            cache[id] = { description: "[loading...]", isFake: true };
            service.getProduct(id).then(function (product) {
                console.log('Product downloaded: ' + product.description);
            });
        }
        return cache[id].description;
    };

    return service;
}]);