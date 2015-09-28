angular.module('workfront-addin', ['ngRoute'])
    .config([
        '$routeProvider', function($routeProvider) {
            $routeProvider
                .when('#/login', {
                    templateUrl: 'Templates/Login/Index.html'
                })
                .when('#/update', {
                    templateUrl: 'Templates/Update/Index.html',
                    controller: 'UpdateController'
                });
        }
    ])
    .run([
        '$rootScope', '$location', function($rootScope, $location) {
            $rootScope.$on("$routeChangeStart", function(event, next, current) {
                        $location.path("#/login");
            });
        }
    ]);