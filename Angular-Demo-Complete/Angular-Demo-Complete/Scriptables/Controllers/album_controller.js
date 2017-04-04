(function () {
    'use strict';

    angular
        .module('Controllers')
        .controller('album_controller', album_controller);

    album_controller.$inject = ['$scope', '$routeParams','SessionState', '$http'];

    function album_controller($scope, $routeParams, SessionState, $http) {

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

        
        
    }
})();
