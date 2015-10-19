(function (angular) {
    'use strict';

    function updateController($scope, $location, $uibModal, entityService, currentItemService) {

        var EMPTYGUID = '00000000-0000-0000-0000-000000000000';

        function buildUpdate(data) {
            return 'From: ' + data.mailbox.name + '(' + data.mailbox.address + ')\r\n' +
                'Date: ' + new Date(data.date) + '\r\n' +
                'Subject: ' + data.subject + '\r\n' +
                '"' + data.body + '"';
        };

        $scope.selected = {};

        $scope.getEntities = function (term) {
            if ($.cookie('workfront-session')) {
                return entityService.getEntities(term)
                    .then(function (data) {
                        return data;
                    }, function (reason) {
                        if (reason.status === 500
                            && reason.data.ExceptionMessage === 'You are not currently logged in') {
                            $uibModal.open({
                                templateUrl: 'templates/login/login.view.html',
                                controller: 'LoginController',
                                backdrop: 'static'
                            });
                        }
                    });
            } else {
                $uibModal.open({
                    templateUrl: 'templates/login/login.view.html',
                    controller: 'LoginController',
                    backdrop: 'static'
                });
                //$location.path('/login');
                return {};
            };
        };

        $scope.getAttachments = (function () {
            var attacments = [];
            attacments.push({ id: EMPTYGUID, name: 'current.eml' });
            $.merge(attacments, currentItemService.getAttachments());
            return attacments;
        })();

        $scope.hasAttachments = function () {
            return $scope.getAttachments.length > 0;
        };

        $scope.removeItem = function (removeItem) {
            $scope.getAttachments = $.grep($scope.getAttachments, function (value) {
                return value.id !== removeItem.id;
            });
        };

        currentItemService.getBody().then(function (data) {
            $scope.getBody = buildUpdate(data);
        });

        $scope.isPublic = false;

        var note = {
            OwnerID: '',
            IsPrivate: !$scope.isPublic,
            NoteText: $scope.getBody,
            entryDate: new Date(),
            ObjID: $scope.selected.id,
            NoteObjCode: $scope.selected.objCode
        };


        function updateEntityAsync(data) {
            var fullNote = {
                note: note,
                attachments: $scope.getAttachments,
                ewsCredentials: {
                    attachmentToken: data,
                    ewsId: currentItemService.getItemId(),
                    ewsUrl: currentItemService.getEwsUrl()
                }
            }

            entityService.updateEntity(fullNote)
                .done(function(respnse) {

                }).fail(function(error) {

                });
        };


        $scope.update = function() {
            currentItemService.getCallbackTokenAsync()
                .done(updateEntityAsync(result));
        };
    }

    angular.module('workfront-addin')
    .controller('UpdateController', updateController);
    updateController.$inject = ['$scope', '$location', '$uibModal', 'entityService', 'currentItemService'];
})(window.angular);