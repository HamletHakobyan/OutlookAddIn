'use strict';

angular.module('workfront-addin', ['ngSanitize', 'ui.select'])
    .controller('UpdateController', function ($scope, $http, $timeout) {
        $http.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';

        $scope.disabled = undefined;
        $scope.searchEnabled = undefined;

        $scope.enable = function () {
            $scope.disabled = false;
        };

        $scope.disable = function () {
            $scope.disabled = true;
        };

        $scope.enableSearch = function () {
            $scope.searchEnabled = true;
        }

        $scope.disableSearch = function () {
            $scope.searchEnabled = false;
        }

        $scope.clear = function () {
            $scope.person.selected = undefined;
            $scope.address.selected = undefined;
            $scope.country.selected = undefined;
        };

        $scope.groupFunc = function (item) {
            if (item.ObjCode === 'PROJ')
                return 'Projects';
            if (item.ObjCode === 'TASK')
                return 'Tasks';
            if (item.ObjCode === 'OPTASK')
                return 'Issues';
        };

        $scope.namedEntities = [{ Name: 'No match found' }];
        $scope.refreshNamedEntities = function (term) {
            return $http.post('../../api/get/containssearch', '=' + term)
                .then(function (response) {
                    if (response.data.length === 0) {
                        $scope.namedEntities = [{ Name: 'No match found' }];
                    } else {
                        $scope.namedEntities = response.data;
                    }
                });
        };
    });
