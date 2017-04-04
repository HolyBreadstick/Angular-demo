(function () {
    'use strict';

    angular
        .module('Filters')
        .filter('price', price);

    price.$inject = ['$filter'];

    //Why format the price on the server, when it can happen in real-time on the client-side
    function price($filter) {
        return function (x) {
            var text = "";
            if (angular.isNumber(x)) {
                if (x === 0) {
                    text = "FREE!";
                } else {
                    text = $filter('currency')(x);
                }
            }

            return text;
        };
    }
})();