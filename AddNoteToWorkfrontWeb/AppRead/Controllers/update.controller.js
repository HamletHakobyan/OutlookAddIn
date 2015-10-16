(function() {
    'use strict';

    angular.module('workfront-addin')
        .controller('UpdateController', [
        '$scope', '$location','entityService', function($scope, $location, entityService) {

            $scope.getEntities = function(term) {
                if ($.cookie('workfront-session')) {
                    return entityService.getEntities(term);
                } else {
                    $location.path('/login');
                    return {};
                };
            }

            $scope.attachments= function() {
                
            }
        }
    ]);
})();