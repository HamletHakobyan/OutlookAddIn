(function(angular) {
    'use strict';

    function routeConfigure($routeProvider, $locationProvider) {
        $routeProvider
            //.when('/login', {
            //    templateUrl: 'templates/modalContainer.html',
            //    controller: 'ModalController'
            //})
            .when('/update', {
                templateUrl: 'templates/update/update.view.html',
                controller: 'UpdateController'
            })
            .when('/updated', {
                templateUrl: 'templates/updated/updated.view.html',
                controller: 'UpdatedController',
                resolve: {
                    entityObject: function (entityService) {
                        return entityService.getObject;
                    }
                }
            })
            .otherwise({ redirectTo: '/update' });
    }

    //conf.run(['$rootScope', '$location', eventHandler]);

    //function eventHandler($rootScope, $location) {
    //    $rootScope.$on("$routeChangeStart", function(event, next, current) {
    //        $location.path("/login");
    //    });
    //}

    angular.module('workfront-addin')
        .config(routeConfigure);
    routeConfigure.$inject = ['$routeProvider', '$locationProvider'];
})(window.angular);

