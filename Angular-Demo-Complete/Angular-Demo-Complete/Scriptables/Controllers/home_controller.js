(function () {
    'use strict';

    angular
        .module('Controllers')
        .controller('home_controller', home_controller);

    home_controller.$inject = ['$scope'];

    function home_controller($scope) {
        $scope.title = 'home_controller';

        activate();

        function activate() { }
    }
})();
