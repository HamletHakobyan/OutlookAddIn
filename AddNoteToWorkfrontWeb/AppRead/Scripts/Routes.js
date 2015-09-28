(function() {
    'use strict';

    var app = angular.module('workfront-addin', ['ngRoute']);
    var conf = app.config(['$routeProvider', '$locationProvider', routeConfigure]);

    function routeConfigure($routeProvider, $locationProvider) {
        $routeProvider
            .when('/login', {
                templateUrl: 'templates/login/index.html',
                controller: 'LoginController'
            })
            .when('/update', {
                templateUrl: 'templates/update/index.html',
                controller: 'UpdateController'
            })
            .otherwise({ redirectTo: '/update' });
    };
})();
//conf.run(['$rootScope', '$location', eventHandler]);

//function eventHandler($rootScope, $location) {
//    $rootScope.$on("$routeChangeStart", function(event, next, current) {
//        $location.path("/login");
//    });
//}

