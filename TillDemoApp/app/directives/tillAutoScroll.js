'use strict';

angular.module('tillDempApp')
.directive('tillAutoScroll', function () {
    return {
        link: function postLink(scope, element, attrs) {
            scope.$watch(
              function () {
                  return element.children().length;
              },
              function () {
                  element.animate({ scrollTop: element.prop('scrollHeight') }, 1000);
              }
            );
        }
    };
});