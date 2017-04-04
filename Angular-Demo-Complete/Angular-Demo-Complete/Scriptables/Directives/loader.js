﻿(function () {
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

        var directive = {
            templateUrl: "/Pages/Templates/Directives/Loader/Loader.html",
            replace: false,
            restrict: "E"
        };



        return directive;
    }

})();