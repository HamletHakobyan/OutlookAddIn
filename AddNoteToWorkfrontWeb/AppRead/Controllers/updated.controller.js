(function (angular) {
    'use strict';

    function updatedController($scope, entityObject) {
        $scope.entity = entityObject();
    }

    angular.module('workfront-addin')
        .controller('UpdatedController', updatedController);

    updatedController.$inject = ['$scope', 'entityObject'];
})(window.angular);