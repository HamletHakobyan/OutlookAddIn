(function() {
    'use strict';

    var app = angular.module('workfront-addin');
    app.requires.push('ngSanitize', 'ui.select');

    app.controller('UpdateController', [
        '$scope', '$http', '$location', '$timeout', function ($scope, $http, $location, $timeout) {
            $http.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';

            $scope.disabled = undefined;
            $scope.searchEnabled = undefined;

            $scope.enable = function() {
                $scope.disabled = false;
            };

            $scope.disable = function() {
                $scope.disabled = true;
            };

            $scope.enableSearch = function() {
                $scope.searchEnabled = true;
            }

            $scope.disableSearch = function() {
                $scope.searchEnabled = false;
            }

            $scope.clear = function() {
                $scope.person.selected = undefined;
                $scope.address.selected = undefined;
                $scope.country.selected = undefined;
            };

            $scope.groupFunc = function(item) {
                if (item.ObjCode === 'PROJ')
                    return 'Projects';
                if (item.ObjCode === 'TASK')
                    return 'Tasks';
                if (item.ObjCode === 'OPTASK')
                    return 'Issues';
                return 'Unknowon Object';
            };

            $scope.namedEntities = [{ Name: 'No match found' }];
            $scope.refreshNamedEntities = function (term) {
                if ($.cookie('workfront-session')) {
                    return $http.post('../../api/get/containssearch', '=' + term)
                        .then(
                            function (response) {
                                if (response.data.length === 0) {
                                    $scope.namedEntities = [{ Name: 'No match found' }];
                                } else {
                                    $scope.namedEntities = response.data;
                                }
                            },
                            function (error) {
                                var e = error;
                            });

                } else {
                    $location.path('/login');
                }
            };
        }
    ]);
})();