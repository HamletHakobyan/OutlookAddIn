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
                objName = 'Unknown Object';
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
                                    objCode: e.ObjCode,
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

        var entityObject;

        var updateEntity = function (updateData) {
            var code = updateData.Note.NoteObjCode;
            entityObject = {
                id: updateData.Note.ObjID,
                objCode: code === 'PROJ' ? 'project' :
                    code === 'TASK' ? 'task' : 'issue',
                host: $.cookie('workfront-host')
            }
            var data = $.param(updateData);
            return $http.post('../../api/get/updateEntity', data);
        }

        var getObject = function() {
            return entityObject;
        }

        // return service
        var service = {};

        service.getEntities = getEntities;
        service.updateEntity = updateEntity;
        service.getObject = getObject;
        return service;
    }

    angular.module('workfront-addin')
        .factory('entityService', entityService);
    entityService.$inject = ['$http', '$q'];

})(window.angular);