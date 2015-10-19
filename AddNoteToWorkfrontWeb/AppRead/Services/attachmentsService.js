(function (angular) {
    'use strict';

    function attachmentsService($http) {

        //return service;

        var service = {};

        // add service methods
        // service.serviceMethod = someServiceMethod;

        return service;
    }

    angular.module('workfront-addin')
        .factory('attachmentsService', attachmentsService);
    // inject here
    attachmentsService.$inject = ['$http'];
})(window.angular)