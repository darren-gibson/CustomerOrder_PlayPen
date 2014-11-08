'use strict';

// Declare app level module which depends on views, and components
angular.module('tillDempApp', [
        'ngRoute',
        'angular-growl',
        'tillDempApp.selling',
        'tillDempApp.payment',
        'tillDempApp.version'
]).
    config([
        '$routeProvider', function ($routeProvider) {
            $routeProvider.otherwise({ redirectTo: '/selling' });
        }
    ]).
    run(['$rootScope', function ($rootScope) {
        function makeCorrectNavTabActive(event, next) {
            var urlPart = next.substr(next.lastIndexOf('#'));
            $('ul.nav > li').removeClass('active');
            $(".nav > li [href='" + urlPart + "']").parent().addClass('active');
        }

        $rootScope.$on("$locationChangeSuccess", makeCorrectNavTabActive);
    }]);