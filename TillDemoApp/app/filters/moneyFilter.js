angular.module('tillDempApp').filter("money", function () {
    return function(money) {
        if (money == undefined)
            return '';
        return '\u00A3' + numeral(money.amount).format('0,0.00');
    };
});