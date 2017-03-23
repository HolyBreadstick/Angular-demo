(function () {
    'use strict';

    angular
        .module('Services')
        .service('SessionState', SessionState);

    SessionState.$inject = ['$location'];

    function SessionState($location) {

        var State = {
            Session: {
                Home: "#/",
                SessionStart: "",
                ForceSSL: false,
                Loading: false,
                LoadingFunction: null,
                StatusMessage: ""
            },
            Errors: {
                
            },
            Endpoint: 'https://localhost:44375'
        };



        //Run time checks for settings




        if (State.Session.ForceSSL == true) {
            var forceSSL = function () {
                if ($location.protocol() !== 'https') {
                    window.location.href = $location.absUrl().replace('http', 'https');
                }
            };
            forceSSL();
        }

        //==========================

        




        State.getData = function () {
            return State;
        };
        
        return State;
    }
})();