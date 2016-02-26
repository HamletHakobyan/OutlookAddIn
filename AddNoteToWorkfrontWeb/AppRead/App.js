(function(angular) {
    'use strict';

    angular.module('workfront-addin', ['ngAnimate', 'ui.bootstrap', 'ngRoute']);
    angular.module('workfront-addin')
        .config([
            '$httpProvider', function($httpProvider) {
                $httpProvider.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
            }
        ]);
})(window.angular);