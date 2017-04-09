(function () {
    'use strict';

    angular
        .module('Services')
        .service('SessionState', SessionState);

    SessionState.$inject = ['$location', '$rootScope'];

    function SessionState($location, $rootScope) {

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
            Endpoint: 'http://localhost:50569/',
            ArtistData: {
                Current: {},
                Search: [],
                All: [],
                MusicAdd: "",
                MusicSearch: "",
                MusicAddLoading: false
            }
        };

        //'http://localhost:50569/'
        //'http://music.baileysproject.com/'


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

        //Navigation functions

        State.Navigate = function (path, id) {
            
            if (id != undefined | id != null) {
                $location.path(path + "/" + id)
            } else if (path != undefined | path != null) {
                $location.path(path);
            }
        };

        State.StartLoading = function () {

            State.Session.Loading = true;
            
        };

        State.EndLoading = function () {
            State.Session.Loading = false;
        };
        //======================================
        




        State.getData = function () {
            return State;
        };
        
        return State;
    }
})();
