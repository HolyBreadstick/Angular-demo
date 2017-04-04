(function () {
    'use strict';

    angular
        .module('Controllers')
        .controller('home_controller', home_controller);

    home_controller.$inject = ['$scope', '$http', 'SessionState'];

    function home_controller($scope, $http, SessionState) {

        $scope.State = SessionState.getData();

        $scope.ArtistData = {
            Current: {},
            Search: []
        };

        $scope.SearchArtist = function (artist) {
            return $http({
                method: "POST",
                url: SessionState.Endpoint + "/api/Artist/Search?Artist=" + artist
            }).then(function (success) {
                //$scope.CurrentArtist = success.data;
                $scope.ArtistData.Search = success.data;
                return success.data;
                })
        };


        $scope.AddArtist = function (Artist) {
            return $http({
                method: "POST",
                url: SessionState.Endpoint + "/api/Artist/Add?Artist=" + Artist                
            }).then(function (success) {
                //console.log(success.data);
                $scope.MusicAdd = "";
                });
        };

        $scope.Clear = function (Field) {
            $scope[Field] = "";
        };

        $scope.Temp = {
            Image: 'http://www.joshuacasper.com/contents/uploads/joshua-casper-samples-free.jpg',
            Artist: "Bailey",
            Album: "Miller"
        };


        $scope.$watch('MusicSearch', function (New, Old) {
            if (New != undefined) {
                $scope.ArtistData.Current = New;
                console.log($scope.ArtistData);
            } else {
                $scope.ArtistData.Current = null;
            }
        });

    }
})();
