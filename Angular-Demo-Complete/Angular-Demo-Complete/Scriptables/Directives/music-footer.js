(function () {
    'use strict';

    angular
        .module('Routing_Main')
        .directive('music_footer', music_footer);

    music_footer.$inject = ['$window'];

    function music_footer($window) {
        // Usage:
        //     <music_footer></music_footer>
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'EA',
            templateUrl:'Pages/Templates/Footer/footer.html'
        };
        return directive;

        function link(scope, element, attrs) {
            console.log();
        }
    }

})();