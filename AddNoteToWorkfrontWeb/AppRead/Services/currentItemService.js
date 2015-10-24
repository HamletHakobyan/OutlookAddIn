(function (angular) {
    'use strict';

    function currentItemService($q) {

        function parseEmail(value) {
            var m = 'http://schemas.microsoft.com/exchange/services/2006/messages';
            var t = 'http://schemas.microsoft.com/exchange/services/2006/types';
            var email = {};
            var xmlDoc = $.parseXML(value);
            var message = xmlDoc.getElementsByTagNameNS(m, 'Items')[0]
                .getElementsByTagNameNS(t, 'Message')[0];
            email.subject = message.getElementsByTagNameNS(t, 'Subject')[0].textContent;
            email.body = message.getElementsByTagNameNS(t, 'Body')[0].textContent;
            email.date = message.getElementsByTagNameNS(t, 'DateTimeSent')[0].textContent;
            var mailBox = message.getElementsByTagNameNS(t, 'From')[0]
                .getElementsByTagNameNS(t, 'Mailbox')[0];
            email.mailbox = {};
            email.mailbox.name = mailBox.getElementsByTagNameNS(t, 'Name')[0].textContent;
            email.mailbox.address = mailBox.getElementsByTagNameNS(t, 'EmailAddress')[0].textContent;
            return email;
        };

        function getSoapEnvelope(request) {
            // Wrap an Exchange Web Services request in a SOAP envelope.
            var result =
                '<?xml version="1.0" encoding="utf-8"?>' +
                    '<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"' +
                    '               xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/"' +
                    '               xmlns:xsd="http://www.w3.org/2001/XMLSchema"' +
                    '               xmlns:t="http://schemas.microsoft.com/exchange/services/2006/types">' +
                    '  <soap:Header>' +
                    '     <t:RequestServerVersion Version="Exchange2013"/>' +
                    '  </soap:Header>' +
                    '  <soap:Body>' +
                    request +
                    '  </soap:Body>' +
                    '</soap:Envelope>';

            return result;
        }

        function getRequest(id) {
            // Return a GetItem EWS operation request for the headers of the specified item.
            var result =
                '    <GetItem xmlns="http://schemas.microsoft.com/exchange/services/2006/messages">' +
                    '     <ItemShape>' +
                    '       <t:BaseShape>IdOnly</t:BaseShape>' +
                    '       <t:BodyType>Text</t:BodyType>' +
                    '       <t:AdditionalProperties>' +
                    '           <t:FieldURI FieldURI="item:Body"/>' +
                    '           <t:FieldURI FieldURI="item:Subject"/>' +
                    '           <t:FieldURI FieldURI="item:DateTimeSent"/>' +
                    '           <t:FieldURI FieldURI="message:From"/>' +
                    '       </t:AdditionalProperties>' +
                    '     </ItemShape>' +
                    '     <ItemIds><t:ItemId Id="' + id + '"/></ItemIds>' +
                    '   </GetItem>';

            return result;
        }

        function getItem() {
            return Office.context.mailbox.item;
        }

        // service interface

        function getItemId() {
            return Office.context.mailbox.item.itemId;
        }

        function getMailbox() {
            return Office.context.mailbox;
        }

        function getEwsUrl() {
            return Office.context.mailbox.ewsUrl;
        }

        function getAttachments() {
            return getItem().attachments;
        }

        function getBody() {
            var defered = $q.defer();
            var requestEnvelope = getSoapEnvelope(getRequest(getItemId(), 'item:Body'));
            var mailbox = getMailbox();
            mailbox.makeEwsRequestAsync(requestEnvelope, function (result) {
                if (result.status === 'succeeded') {
                    var email = parseEmail(result.value);
                    defered.resolve(email);
                } else {
                    defered.reject(result.error);
                }
            });

            return defered.promise;
        }

        function getTokenAsync() {
            var defered = $q.defer();

            getMailbox().getCallbackTokenAsync(function(result) {
                if (result.status === 'succeeded') {
                    defered.resolve(result.value);
                } else {
                    defered.reject(result.error);
                }
            });

            return defered.promise;
        }


        //return service;

        var service = {};

        // add service methods
        service.getAttachments = getAttachments;
        service.getBody = getBody;
        service.getEwsUrl = getEwsUrl;
        service.getItemId = getItemId;
        service.getTokenAsync = getTokenAsync;

        return service;
    }

    angular.module('workfront-addin')
        .factory('currentItemService', currentItemService);
    // inject here
    currentItemService.$inject=['$q'];
})(window.angular)