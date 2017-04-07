(function () {
    'use strict';

    angular
        .module('Routing_Main')
        .directive('song', song);

    song.$inject = ['$window', 'SessionState' ,'$sce'];

    function song($window, SessionState, $sce) {
        // Usage:
        //     <musicDisplay></musicDisplay>
        // Creates:
        // 

        var directive = {
            templateUrl: "/Pages/Templates/Directives/Song/Song.html?Version=1.11",
            scope: {
                data: "="
            },
            replace: false,
            restrict: "E",
            link: function ($scope, elements, attrs) {
                $scope.CompleteLink = $sce.trustAsResourceUrl('https://www.youtube.com/embed/' + $scope.data.YoutubeLink[0].Link + '/');
                $scope.show = false;

                $scope.ToggleVideo = function () {
                    if ($scope.show == true) {
                        $scope.show = false;
                    } else {
                        $scope.show = true;
                    }
                };
            }
        };



        return directive;
    }

})();