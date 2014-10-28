angular.module('tillDempApp').filter("prodDesc", ['productInfo', function (productInfo) {
    return function (id) {
        return productInfo.getDescription(id);
    };
}]);