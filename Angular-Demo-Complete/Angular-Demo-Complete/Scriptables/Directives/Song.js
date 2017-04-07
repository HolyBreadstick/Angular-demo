(function () {
    'use strict';

    angular
        .module('Routing_Main')
        .directive('song', song);

    song.$inject = ['$window', 'SessionState' ,'$sce', '$compile', '$rootScope'];

    function song($window, SessionState, $sce, $compile, $rootScope) {
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
                        $rootScope.$broadcast('SongEnded', {
                            ID: $scope.data.ID
                        });
                        $scope.show = false;
                    } else {
                        $scope.show = true;
                    }
                };

                $scope.$on('youtube.player.ready', function ($event, player) {
                    //console.log(player);
                    //console.log($event);
                    player.playVideo();
                });

                $scope.$on('youtube.player.ended', function ($event, player) {
                    //console.log(player);
                    $scope.ToggleVideo();
                });

                $rootScope.$on('Toggle', function (event, data) {
                    if ($scope.data.ID == data.ID) {
                        $scope.ToggleVideo();
                    }
                });
            }
        };



        return directive;
    }

})();