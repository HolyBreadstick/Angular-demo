(function () {
    'use strict';

    angular
        .module('Controllers')
        .controller('main_controller', main_controller);

    main_controller.$inject = ['$scope'];

    function main_controller($scope) {
        $scope.title = 'main_controller';

        activate();

        function activate() { }
    }
})();
