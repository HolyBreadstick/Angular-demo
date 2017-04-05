(function () {
    'use strict';

    angular
        .module('Controllers')
        .controller('home_controller', home_controller);

    home_controller.$inject = ['$scope', '$http', 'SessionState', '$interval', '$rootScope'];

    function home_controller($scope, $http, SessionState, $interval, $rootScope) {

        $scope.State = SessionState.getData();
        $scope.Searchables = [];
        $scope.SearchPromise = {};
        $scope.LoadAll = function () {
            return $http({
                method: "GET",
                url: SessionState.Endpoint + "api/Artist/All"
            }).then(function (success) {
                $scope.Searchables = success.data;
                if (SessionState.ArtistData.All.length == 0 | SessionState.ArtistData.All.length < 25) {
                    $scope.SearchPromise = $interval(albumWorker, 1000, 25, true);
                }
                });
        };

        function albumWorker() {
            $scope.AlbumSearch();
        }

        $rootScope.$on("CancelSearch", function () {
            $interval.cancel($scope.SearchPromise);
        });

        $scope.AlbumSearch = function () {
            return $http({
                url: SessionState.Endpoint + "api/Artist/Album/Search?Album=" + $scope.Searchables.shift(),
                method: "POST"
            }).then(function (success) {
                SessionState.ArtistData.All.push(success.data);
            });
        };

        $scope.SearchArtist = function (artist) {
            return $http({
                method: "POST",
                url: SessionState.Endpoint + "/api/Artist/Search?Artist=" + artist
            }).then(function (success) {
                //$scope.CurrentArtist = success.data;
                $scope.State.ArtistData.Search = success.data;
                return success.data;
                })
        };


        $scope.AddArtist = function (Artist) {
            $scope.State.ArtistData.MusicAddLoading = true;
            return $http({
                method: "POST",
                url: SessionState.Endpoint + "/api/Artist/Add?Artist=" + Artist                
            }).then(function (success) {
                //console.log(success.data);
                $scope.State.ArtistData.MusicAdd = "";
                $scope.State.ArtistData.MusicAddLoading = false;
                });
        };

        $scope.Clear = function (Field) {
            $scope.State.ArtistData[Field] = "";
        };

        


        $scope.$watch('State.ArtistData.MusicSearch', function (New, Old) {
            if (New != undefined & New != "") {
                $scope.State.ArtistData.Current = New;
                $interval.cancel($scope.SearchPromise);
                console.log($scope.ArtistData);
            } else {
                $scope.State.ArtistData.Current = null;
            }
        });

        $scope.LoadAll();
    }
})();
