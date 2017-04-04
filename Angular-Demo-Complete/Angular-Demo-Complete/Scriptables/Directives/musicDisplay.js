(function () {
    'use strict';

    angular
        .module('Routing_Main')
        .directive('musicDisplay', musicDisplay);

    musicDisplay.$inject = ['$window'];

    function musicDisplay($window) {
        // Usage:
        //     <musicDisplay></musicDisplay>
        // Creates:
        // 
        
        var directive = {
            templateUrl: "/Pages/Templates/Directives/MusicDisplay/MusicDisplay.html",
            scope: {
                data: "="
            },
            replace: false,
            restrict: "E"
        };



        return directive;
    }

})();