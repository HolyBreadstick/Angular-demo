(function () {
    'use strict';

    angular
        .module('Routing_Main')
        .directive('song', song);

    song.$inject = ['$window', 'SessionState' ,'$sce', '$compile', '$rootScope', '$http'];

    function song($window, SessionState, $sce, $compile, $rootScope, $http) {
        // Usage:
        //     <musicDisplay></musicDisplay>
        // Creates:
        // 

        var directive = {
            templateUrl: "/Pages/Templates/Directives/Song/Song.html?Version=1.16",
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

                $scope.Download = function () {
                    $scope.ToggleVideo();
                    return $http({
                        url: SessionState.Endpoint + "/api/Information/Download?link=https://www.youtube.com/watch?v=" + $scope.data.YoutubeLink[0].Link + "&wantVideo=false",
                        method: "POST"
                    }).then(function (data) {
                        console.log(data);
                        

                        var sampleBytes = data.data.Link;

                        var saveByteArray = (function () {
                            var a = document.createElement("a");
                            document.body.appendChild(a);
                            a.style = "display: none";
                            return function (data, name) {
                                a.href = data;
                                a.download = name;
                                a.click();
                            };
                        }());

                        saveByteArray([sampleBytes], data.data.FileName);


                        });
                };

            }
        };



        return directive;
    }

})();