(function () {
    'use strict';



    //Typically I don't declare variables however we need to use this specific module twice in this same file.
    var app = angular.module('Routing_Main', [
        // Angular modules 
        'ngRoute',
        // Custom modules 
        'Controllers',
        'Filters'
        // 3rd Party Modules

    ]);

    //The config method accepts a function
    //This function requires $routeProvider as a parameter or dependency
    //This dependency will be the Angular tool to configure what hash or route resolves to what view
    app.config(function ($routeProvider) {


        //Lets declare a starting route
        //There is another method for declaring the starting route
        //The other method is a global wrapper that uses if not route match go here
        //I prefer to see the actual route and what template and controller it has
        $routeProvider
            .when('/', {
                templateUrl: 'Pages/home.html?Version=1.10', //Why a query string? This way we can force browser to download new 'versions' as we make big changes
                controller: 'home_controller'
            })
            .when('/album/:id/', {
                templateUrl: 'Pages/AlbumSearch.html?Version=1.10',
                controller: 'album_controller'
            });
    });
    

})();