(function() {
    'use strict';

    var app = angular.module('workfront-addin');
    app.requires.push('ngAnimate', 'ui.bootstrap');

    app.controller('UpdateController', [
        '$scope', '$http', '$location', function($scope, $http, $location) {
            $http.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';

            var groupFunc = function(item) {
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

            $scope.values = [
                {
                    id: '1',
                    name: 'asdkfjhasfd',
                    group_name: 'Issues',
                    is_first: true
                },
                {
                    id: '2',
                    name: 'asdfasdfaasdkfjhasfd',
                    group_name: 'Projects',
                    is_first: true
                }
            ];

            $scope.selected = undefined;
            // Any function returning a promise object can be used to load values asynchronously
            $scope.getEntities = function(term) {
                if ($.cookie('workfront-session')) {
                    return $http.post('../../api/get/containssearch', '=' + term)
                        .then(function(response) {
                                var data = response.data;
                                var ldata = _(data);
                                var group = ldata.groupBy(groupFunc);
                                var grouopMap = group.map(function(g) {
                                    _.each(g, function(e, i) {
                                        //          console.log(e);
                                        var isFirst = false;
                                        if (i === 0) {
                                            isFirst = true;
                                        }

                                        e.is_first = isFirst;
                                    });

                                    return g;
                                });
                                var flattened = grouopMap.flatten();
                                var res = flattened.map(function(e) {
                                        return {
                                            id: e.ID,
                                            name: e.Name,
                                            group_name: e.obj_name,
                                            is_first: e.is_first
                                        };
                                    })
                                    .value();
                                return res;
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