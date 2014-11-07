'use strict';

angular.module('tillDempApp')
    .controller('tillQuickPaymentsCtrl', ['$scope', 'payment', function ($scope, payment) {
        $scope.quickpayments = {
            payments: [
                { description: "Cash", imageUrl: 'imgs/cash.jpg', tenderType: 'Cash' }
            ]
        };
    }])
    .directive('tillQuickPayments', function () {
        return {
            templateUrl: 'directives/tillQuickPayments.html'
        };
    });