(function () {
    'use strict';

    angular
        .module('Routing_Main')
        .directive('youtubeVideo', youtubeVideo);

    youtubeVideo.$inject = ['$window', '$sce'];

    function youtubeVideo($window, $sce) {
        // Usage:
        //     <youtube-video></youtube-video>
        // Creates:
        // 

        var directive = {
            templateUrl: "/Pages/Templates/Directives/YoutubeVideo/YoutubeVideo.html?Version=1.13",
            replace: false,
            restrict: "E",
            scope: {
                show: "=",
                data: "="
            },
            link: function ($scope, element, attrs) {
                
                $scope.CompleteLink = $sce.trustAsResourceUrl('https://www.youtube.com/embed/' + $scope.data.YoutubeLink[0].Link + '/');
                //$scope.Width = 100;

                $scope.Width = document.getElementsByClassName('song-item')[0].offsetWidth;

                console.log(document.getElementsByClassName('song-item')[0].offsetWidth);

                angular.element($window).bind('resize', function () {
                    $scope.Width = document.getElementsByClassName('song-item')[0].offsetWidth;
                    $scope.$digest();
                });
                
                //Youtube Iframe API
                var player = new YT.Player('player', {
                    height: '390',
                    width: '640',
                    videoId: '0Bmhjf0rKe8',
                    events: {
                        'onReady': onPlayerReady,
                        'onStateChange': onPlayerStateChange
                    }
                });

                function onPlayerReady(event) {
                    event.target.playVideo();
                }

                // when video ends
                function onPlayerStateChange(event) {
                    if (event.data === 0) {
                        alert('done');
                    }
                }
            }
        }
        return directive;
    }

})();