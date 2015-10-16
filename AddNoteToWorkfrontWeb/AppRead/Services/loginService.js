(function() {
    'use strict';

    angular.module('workfront-addin')
    .factory('loginService', [
        '$http', function ($http) {
            var doLogin = function (connectionInfo) {
                var data = $.param(connectionInfo);
                return $http.post("../../api/authentication/login", data);
            };

            return {
                doLogin: doLogin
            };
        }
    ]);
})();
