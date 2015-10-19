﻿(function (angular) {
    'use strict';

    function currentItemService($q) {

        function parseEmail(value) {
            var email = {};
            var xmlDoc = $.parseXML(value);
            var message = $("m\\:Items>t\\:Message", xmlDoc);
            email.subject = $("t\\:Subject", message).text();
            email.body = $("t\\:Body", message).text();
            email.date = $("t\\:DateTimeSent", message).text();
            var mailBox = $("t\\:From>t\\:Mailbox", message);
            email.mailbox = {};
            email.mailbox.name = $("t\\:Name", mailBox).text();
            email.mailbox.address = $("t\\:EmailAddress", mailBox).text();
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

        function getItemId() {
            return Office.context.mailbox.item.itemId;
        }

        function getMailbox() {
            return Office.context.mailbox;
        }

        function getItem() {
            return Office.context.mailbox.item;
        }

        // service interface

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

        //return service;

        var service = {};

        // add service methods
        service.getAttachments = getAttachments;
        service.getBody = getBody;

        return service;
    }

    angular.module('workfront-addin')
        .factory('currentItemService', currentItemService);
    // inject here
    currentItemService.$inject=['$q'];
})(window.angular)