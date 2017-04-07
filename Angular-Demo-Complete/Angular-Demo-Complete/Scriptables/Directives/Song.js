(function () {
    'use strict';

    angular
        .module('Routing_Main')
        .directive('song', song);

    song.$inject = ['$window', 'SessionState' ,'$sce', '$compile'];

    function song($window, SessionState, $sce, $compile) {
        // Usage:
        //     <musicDisplay></musicDisplay>
        // Creates:
        // 

        var directive = {
            templateUrl: "/Pages/Templates/Directives/Song/Song.html?Version=1.13",
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
                        angular.element(document.getElementById($scope.data.ID)).replaceWith(($compile('<div id="{{data.ID}}"></div>')($scope))); 
                        $scope.show = false;
                    } else {
                        angular.element(document.getElementById($scope.data.ID)).html(($compile('<youtube-video data="data" show="show"></youtube-video>')($scope)));
                        $scope.show = true;
                    }
                };
            }
        };



        return directive;
    }

})();