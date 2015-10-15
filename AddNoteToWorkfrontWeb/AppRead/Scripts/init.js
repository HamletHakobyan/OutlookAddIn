/// <reference path="../App.js" />

(function() {
    "use strict";

    var host;

    function getHost() {
        var mailbox = Office.context.mailbox;
        var item = mailbox.item;
        var matches = item.getRegExMatchesByName("CommentOnProject");
        var match = matches[0];
        return match;
    }

    function setHost() {
        host = getHost();
    }

    function doMyWork() {
        $(".loading").show(1000);

        $.post("../../api/get/mywork")
            .done(function(data, status) {
                $("#jsonResponse").text("Data: " + JSON.stringify(data) + "\nStatus: " + status);
            })
            .fail(function(data, status) {
                $("#jsonResponse").text("Data: " + JSON.stringify(data) + "\nStatus: " + status);
            })
            .always(function() {
                $(".loading").hide(1000);
            });
    }

    function doGetObjects() {
        $(".loading").show(1000);
        var searchCriteria = $('#searchCriteria').val();
        $.post("../../api/get/containssearch", { '': searchCriteria })
            .done(function(data, status) {
                $("#jsonResponse").text("Data: " + JSON.stringify(data) + "\nStatus: " + status);
            })
            .fail(function(data, status) {
                $("#jsonResponse").text("Data: " + JSON.stringify(data) + "\nStatus: " + status);
            })
            .always(function() {
                $(".loading").hide(1000);
            });
    }

    function getRequestDataPromise() {
        var itemId = Office.context.mailbox.item.itemId;
        var ewsUrl = Office.context.mailbox.ewsUrl;

        var getCallbackTokenAsync = $.Deferred();
        var getUserIdentityTokenAsync = $.Deferred();

        Office.context.mailbox.getCallbackTokenAsync(function(data) {
            getCallbackTokenAsync.resolve(data);
        });
        Office.context.mailbox.getUserIdentityTokenAsync(function(data) {
            getUserIdentityTokenAsync.resolve(data);
        });

        return $.when(getCallbackTokenAsync.promise(), getUserIdentityTokenAsync.promise()).then(function(callbackToken, userIdentityToken) {
            var postData = {
                AttachmentToken: callbackToken.value,
                UserIdentityToken: userIdentityToken.value,
                EwsId: itemId,
                EwsUrl: ewsUrl
            };

            return postData;
        });
    }
    
    function doGetAttachmentsHandles() {
        $(".loading").show(1000);

        getRequestDataPromise().done(function(postData) {
            $.post("../../api/get/attachmenthandles", postData)
                .done(function(data, status) {
                    $("#jsonResponse").text("Data: " + JSON.stringify(data) + "\nStatus: " + status);
                })
                .fail(function(data, status) {
                    $("#jsonResponse").text("Data: " + JSON.stringify(data) + "\nStatus: " + status);
                })
                .always(function() {
                    $(".loading").hide(1000);
                });

        });
    }

    function getHeaders() {
        var mailbox = Office.context.mailbox;
        var itemId = mailbox.item.itemId;
        var headersRequest = getHeadersRequest(itemId);
        var envelope = getSoapEnvelope(headersRequest);
        var res;
        $(".spinner-loader").show("slow");
        mailbox.makeEwsRequestAsync(envelope, function(result) {
            var xmlDoc = $.parseXML(result.value);
            var $xml = $("t\\:InternetMessageHeader[HeaderName^='X-AtTask-']", xmlDoc);
            $xml.each(function() {
                var header = $(this).attr("HeaderName");
                var val = $(this).text();
                res += "\nHeader - " + header + "     Value - " + val;
            });
            $("textarea#jsonResponse").val(res);
            $(".spinner-loader").hide("slow");
        });

        var username = $("#username").text;
        var password = $("#password").text;
        var instance = new window.Workfront.Api({ url: host, version: "3.0" });
        instance.login(username, password).then(function(data) {
            var value = data.sessionID;
            $("textarea#jsonResponse").val(value);
        }, console.error);
    }

    function getSoapEnvelope(request) {
        // Wrap an Exchange Web Services request in a SOAP envelope.
        var result =
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"" +
                "               xmlns:t=\"http://schemas.microsoft.com/exchange/services/2006/types\">" +
                "  <soap:Header>" +
                "     <t:RequestServerVersion Version=\"Exchange2013\"/>" +
                "  </soap:Header>" +
                "  <soap:Body>" +
                request +
                "  </soap:Body>" +
                "</soap:Envelope>";

        return result;
    }

    function getHeadersRequest(id) {
        // Return a GetItem EWS operation request for the headers of the specified item.
        var result =
            "    <GetItem xmlns=\"http://schemas.microsoft.com/exchange/services/2006/messages\">" +
                "      <ItemShape>" +
                "        <t:BaseShape>IdOnly</t:BaseShape>" +
                "        <t:BodyType>Text</t:BodyType>" +
                "        <t:AdditionalProperties>" +
                "            <t:FieldURI FieldURI=\"item:InternetMessageHeaders\"/>" +
                "        </t:AdditionalProperties>" +
                "      </ItemShape>" +
                "      <ItemIds><t:ItemId Id=\"" + id + "\"/></ItemIds>" +
                "    </GetItem>";

        return result;
    }

    // The Office initialize function must be run each time a new page is loaded
    Office.initialize = function(reason) {
    };
})();