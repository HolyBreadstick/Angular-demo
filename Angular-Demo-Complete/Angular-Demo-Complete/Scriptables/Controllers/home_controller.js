(function () {
    'use strict';

    angular
        .module('Controllers')
        .controller('home_controller', home_controller);

    home_controller.$inject = ['$scope', '$http', 'SessionState'];

    function home_controller($scope, $http, SessionState) {

        $scope.State = SessionState.getData();

        $scope.Static = ["Yes", "No"];

        $scope.SearchArtist = function (artist) {
            return $http({
                method: "POST",
                url: SessionState.Endpoint + "/api/Artist/Search?Artist=" + artist
            }).then(function (success) {
                return success.data;
                })
        };


        $scope.AddArtist = function (Artist) {
            return $http({
                method: "POST",
                url: SessionState.Endpoint + "/api/Artist/Add?Artist=" + Artist                
            }).then(function (success) {
                console.log(success.data);
                });
        };

        $scope.Clear = function (Field) {
            $scope[Field] = "";
        };
    }
})();
