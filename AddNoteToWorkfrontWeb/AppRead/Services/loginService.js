(function (angular) {
    'use strict';

    function loginService($http) {

        this.doLogin = function (connectionInfo) {
            var data = $.param(connectionInfo);
            return $http.post("../../api/authentication/login", data);
        }
    };

    angular.module('workfront-addin')
        .service('loginService', loginService);
    loginService.$inject = ['$http'];

})(window.angular);
