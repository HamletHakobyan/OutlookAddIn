(function() {
    'use strict';

    angular.module('workfront-addin')
        .config(['$routeProvider', '$locationProvider', routeConfigure]);

    function routeConfigure($routeProvider, $locationProvider) {
        $routeProvider
            .when('/login', {
                templateUrl: 'templates/modalContainer.html',
                controller: 'ModalController'
            })
            .when('/update', {
                templateUrl: 'templates/update/update.view.html',
                controller: 'UpdateController'
            })
            .otherwise({ redirectTo: '/update' });
    }
})();
//conf.run(['$rootScope', '$location', eventHandler]);

//function eventHandler($rootScope, $location) {
//    $rootScope.$on("$routeChangeStart", function(event, next, current) {
//        $location.path("/login");
//    });
//}

