(function () {
    'use strict';

    angular.module('workfront-addin', ['ngAnimate', 'ui.bootstrap', 'ngCookies', 'ngRoute'])
    .run(['$http', function ($http) {
        $http.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
    }]);
})();