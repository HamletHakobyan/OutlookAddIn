(function(angular) {
    'use strict';

    function entityService($http, $q) {
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

        var getEntities = function(term) {
            var defered = $q.defer();
            $http.post('../../api/get/containssearch', '=' + term)
                .then(function(response) {
                        var res = _(response.data)
                            .groupBy(groupFunc)
                            .map(function(g) {
                                _.each(g, function(e, i) {
                                    var isFirst = false;
                                    if (i === 0) {
                                        isFirst = true;
                                    }

                                    e.is_first = isFirst;
                                });

                                return g;
                            })
                            .flatten()
                            .map(function(e) {
                                return {
                                    id: e.ID,
                                    name: e.Name,
                                    group_name: e.obj_name,
                                    is_first: e.is_first
                                };
                            })
                            .value();
                        return defered.resolve(res);
                    },
                    function(error) {
                        defered.reject(error);
                    });

            return defered.promise;
        };

        // return service
        var service = {};

        service.getEntities = getEntities;
        return service;
    }

    angular.module('workfront-addin')
        .factory('entityService', entityService);
    entityService.$inject = ['$http', '$q'];

})(window.angular);