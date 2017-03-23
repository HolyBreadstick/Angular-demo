(function () {
    'use strict';

    angular
        .module('Controllers')
        .controller('home_controller', home_controller);

    home_controller.$inject = ['$scope', '$http'];

    function home_controller($scope, $http) {

        $scope.SearchArtist = function (artist) {
            return $http({

            });
        };
    }
})();
