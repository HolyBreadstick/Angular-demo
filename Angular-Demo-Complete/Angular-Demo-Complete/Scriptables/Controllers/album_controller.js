(function () {
    'use strict';

    angular
        .module('Controllers')
        .controller('album_controller', album_controller);

    album_controller.$inject = ['$scope', '$routeParams','SessionState', '$http', '$rootScope'];

    function album_controller($scope, $routeParams, SessionState, $http, $rootScope) {

        $scope.State = SessionState.getData();

        var id = $routeParams.id;
        var Album = {};
        var loadingFunction = function () {
            //Holds logic for actions to happen during page load.
            SessionState.StartLoading();

            return $http({
                url: SessionState.Endpoint + "api/Artist/Album/Search?Album="+id,
                method: "POST"
            }).then(function (success) {
                $scope.Album = success.data;
                console.log($scope.Album);
                SessionState.EndLoading();
                });
        };

        loadingFunction();

        $scope.CurrentSong = {
            Index: 0,
            Song: {}
        };

        $scope.PlayAll = function () {
            $rootScope.$broadcast('Toggle', {
                ID: $scope.Album.Songs[$scope.CurrentSong.Index].ID
            });
            $scope.CurrentSong.Song = $scope.Album.Songs[$scope.CurrentSong.Index];
        };

        $rootScope.$on('SongEnded', function (event, data) {
            $scope.CurrentSong.Index += 1;
            $rootScope.$broadcast('Toggle', {
                ID: $scope.Album.Songs[$scope.CurrentSong.Index].ID
            });
            $scope.CurrentSong.Song = $scope.Album.Songs[$scope.CurrentSong.Index];
        });
        
    }
})();
