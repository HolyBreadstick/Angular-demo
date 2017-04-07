(function () {
    'use strict';

    angular
        .module('Routing_Main')
        .directive('musicFooter', musicFooter);

    musicFooter.$inject = ['$window'];

    function musicFooter($window) {
        // Usage:
        //     <musicFooter></musicFooter>
        // Creates:
        // 
        return {
            templateUrl: "/Pages/Templates/Directives/Footer/footer.html?Version=1.13"
        };
    }

})();