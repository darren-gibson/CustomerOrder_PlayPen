'use strict';

angular.module('tillDempApp').factory('notify', ['growl', function (growl) {
    var service = {};

    service.warn = function(title, message, variables) {
        growl.warning(message, { "ttl": 10000, title: title, "variables": variables });
    };

    service.success = function(title, message, variables) {
        growl.success(message, { "ttl": 3000, title: title, "variables": variables });
    };

    return service;
}]);