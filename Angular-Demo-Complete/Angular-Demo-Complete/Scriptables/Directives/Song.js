(function () {
    'use strict';

    angular
        .module('Routing_Main')
        .directive('song', song);

    song.$inject = ['$window', 'SessionState'];

    function song($window, SessionState) {
        // Usage:
        //     <musicDisplay></musicDisplay>
        // Creates:
        // 

        var directive = {
            templateUrl: "/Pages/Templates/Directives/Song/Song.html",
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