(function (angular) {
    'use strict';

    function loginService($http) {

        function doLogin(connectionInfo) {
            var data = $.param(connectionInfo);
            return $http.post("../../api/authentication/login", data);
        }

        // return service
        var service = {};

        service.doLogin = doLogin;
        return service;
    };

    angular.module('workfront-addin')
        .factory('loginService', loginService);
    loginService.$inject = ['$http'];


})(window.angular);
