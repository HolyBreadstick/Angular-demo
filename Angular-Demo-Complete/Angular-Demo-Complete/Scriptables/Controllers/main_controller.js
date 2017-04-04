(function () {
    'use strict';

    angular
        .module('Controllers')
        .controller('main_controller', main_controller);

    main_controller.$inject = ['$scope', 'SessionState'];

    function main_controller($scope, SessionState) {

        $scope.State = SessionState.getData();

    }
})();
