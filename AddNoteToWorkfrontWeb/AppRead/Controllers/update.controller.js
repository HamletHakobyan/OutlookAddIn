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

        $scope.selected = undefined;

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
            attacments.push({ id: EMPTYGUID, name: 'current.eml', attachmentType: 'file', contentType: 'message/rfc822', isInline: false});
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

        $scope.isPrivate = true;

        function updateEntityAsync(data) {
            var note = {
                OwnerID: '$$USER.ID',
                IsPrivate: $scope.isPrivate,
                NoteText: $scope.getBody,
                EntryDate: new Date().toISOString(),
                ObjID: $scope.selected.id,
                NoteObjCode: $scope.selected.objCode
            };

            var fullNote = {
                Note: note,
                Attachments: $scope.getAttachments.map(function(attachment) {
                    return {
                        Id: attachment.id,
                        Name: attachment.name,
                        AttachmentType: attachment.attachmentType,
                        ContentType: attachment.contentType,
                        IsInline: attachment.isInline
                    };
                }),
                EwsCredentials: {
                    AttachmentToken: data,
                    EwsId: currentItemService.getItemId(),
                    EwsUrl: currentItemService.getEwsUrl()
                }
            }

            entityService.updateEntity(fullNote)
                .done(function(respnse) {

                }).fail(function(error) {

                });
        };


        $scope.update = function() {
            currentItemService.getTokenAsync()
                .then(updateEntityAsync);
        };
    }

    angular.module('workfront-addin')
    .controller('UpdateController', updateController);
    updateController.$inject = ['$scope', '$location', '$uibModal', 'entityService', 'currentItemService'];
})(window.angular);