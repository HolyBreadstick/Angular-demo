(function () {
    'use strict';

    angular
        .module('Routing_Main')
        .directive('loader', loader);

    loader.$inject = ['$window', 'SessionState'];

    function loader($window, SessionState) {
        // Usage:
        //     <loader></loader>
        // Creates:
        // 

        //Testing

        var directive = {
            templateUrl: "/Pages/Templates/Directives/Loader/Loader.html?Version=1.8",
            replace: false,
            restrict: "E",
            scope: {
                height: "@",
                width: "@",
                inline: "@"
            },
            link: function ($scope, element, attrs) {
                if ($scope.inline == 'true') {
                    element.addClass("inline-block");
                }
            }
        };



        return directive;
    }

})();