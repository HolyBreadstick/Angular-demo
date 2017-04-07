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
            templateUrl: "/Pages/Templates/Directives/YoutubeVideo/YoutubeVideo.html?Version=1.12",
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

            }
        };



        return directive;
    }

})();