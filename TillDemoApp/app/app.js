'use strict';

// Declare app level module which depends on views, and components
angular.module('tillDempApp', [
        'ngRoute',
        'angular-growl',
        'tillDempApp.selling',
        'tillDempApp.view2',
        'tillDempApp.version'
    ]).
    config([
        '$routeProvider', function($routeProvider) {
            $routeProvider.otherwise({ redirectTo: '/selling' });
        }
    ]);

