(function () {
    'use strict';

    angular
        .module('Routing_Main')
        .directive('ngEnter', ngEnter);

    ngEnter.$inject = [];

    function ngEnter() {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.myEnter);
                    });

                    event.preventDefault();
                }
            });
        };
    }

})();