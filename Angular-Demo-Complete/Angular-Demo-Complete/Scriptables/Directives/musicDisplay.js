(function () {
    'use strict';

    angular
        .module('Routing_Main')
        .directive('musicDisplay', musicDisplay);

    musicDisplay.$inject = ['$window', 'SessionState'];

    function musicDisplay($window, SessionState) {
        // Usage:
        //     <musicDisplay></musicDisplay>
        // Creates:
        // 
        
        var directive = {
            templateUrl: "/Pages/Templates/Directives/MusicDisplay/MusicDisplay.html?Version=1.15",
            scope: {
                data: "="
            },
            replace: false,
            restrict: "E",
            link: function ($scope, elements, attrs) {
                $scope.Navigate = SessionState.Navigate;
            }
        };



        return directive;
    }

})();