(function () {
    'use strict';

    var app = angular.module('workfront-addin');
    app.requires.push('ngAnimate', 'ui.bootstrap');

    app.controller('UpdateController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
            $http.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';

            $scope.groupFunc = function (item) {
                var objName;
                if (item.ObjCode === 'PROJ') {
                    objName = 'Projects';
                } else if (item.ObjCode === 'TASK') {
                    objName = 'Tasks';
                } else if (item.ObjCode === 'OPTASK') {
                    objName = 'Issues';
                } else {
                    objName = 'Unknowon Object';
                }

                item.obj_name = objName;
                return objName;
            };


            $scope.selected = undefined;
            // Any function returning a promise object can be used to load values asynchronously
            $scope.getEntities = function (term) {
                if ($.cookie('workfront-session')) {
                    return $http.post('../../api/get/containssearch', '=' + term)
                        .then(function(response) {
                                var data = response.data;
                                _(data).groupBy(groupFunc)
                                    .map(function(g) {
                                        _.each(g, function(e, i) {
                                            //          console.log(e);
                                            var isFirst = false;
                                            if (i === 0) {
                                                isFirst = true;
                                            }

                                            e.is_first = isFirst;
                                        });

                                    })
                                    .flatten()
                                    .map(function(e) {
                                        return {
                                            id: e.ID,
                                            name: e.Name,
                                            group_name: e.Obj_name,
                                            is_first: e.is_first
                                        };
                                    })
                                    .value();
                            },
                            function(error) {
                                var e = error;
                            });

                } else {
                    $location.path('/login');
                    return {};
                }
            };
        }
    ]);
})();