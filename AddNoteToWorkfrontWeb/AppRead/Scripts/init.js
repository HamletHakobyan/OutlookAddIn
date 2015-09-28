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

    function doLogin() {
        var connectionInfo = {
            Username: $("#username").val(),
            Password: $("#password").val(),
            Host: host
        };

        $(".loading").show(1000);

        $.post("../../api/authentication/login", connectionInfo)
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
        $(document).ready(function() {
            //var data =
            //[
            //    {
            //        id:""
            //    },
            //    {
            //        id: "100",
            //        text: "Gastos",
            //        children: [
            //            { id: "2", text: "PESSOAL" },
            //            { id: "3", text: "SALÁRIOS" },
            //            { id: "4", text: "DIVIDENDOS / COMISSÕES /BONUS" },
            //            { id: "5", text: "INSS" },
            //            { id: "6", text: "FGTS" }
            //        ]
            //    },
            //    {
            //        id: "200",
            //        text: "Chitos",
            //        children: [
            //            { id: "7", text: "PESSOAL" },
            //            { id: "8", text: "SALÁRIOS" },
            //            { id: "9", text: "DIVIDENDOS / COMISSÕES /BONUS" },
            //            { id: "10", text: "INSS" },
            //            { id: "11", text: "FGTS" }
            //        ]

            //    }
            //];

            //$('#txtConta').select2({
            //    allowClear: true,
            //    width: '330px',
            //    placeholder: 'Select an options...',

            //    ajax: {
            //        // The number of milliseconds to wait for the user to stop typing before
            //        // issuing the ajax request.
            //        delay: 250,
            //        // You can craft a custom url based on the parameters that are passed into the
            //        // request. This is useful if you are using a framework which has
            //        // JavaScript-based functions for generating the urls to make requests to.
            //        //
            //        // @param params The object containing the parameters used to generate the
            //        //   request.
            //        // @returns The url that the request should be made to.
            //        url: '../../api/get/containssearch',
            //        // You can pass custom data into the request based on the parameters used to
            //        // make the request. For `GET` requests, the default method, these are the
            //        // query parameters that are appended to the url. For `POST` requests, this
            //        // is the form data that will be passed into the request. For other requests,
            //        // the data returned from here should be customized based on what jQuery and
            //        // your server are expecting.
            //        //
            //        // @param params The object containing the parameters used to generate the
            //        //   request.
            //        // @returns Data to be directly passed into the request.
            //        data: function(params) {
            //            return params.term;
            //        },

            //        type: 'POST',
            //        // You can modify the results that are returned from the server, allowing you
            //        // to make last-minute changes to the data, or find the correct part of the
            //        // response to pass to Select2. Keep in mind that results should be passed as
            //        // an array of objects.
            //        //
            //        // @param data The data as it is returned directly by jQuery.
            //        // @returns An object containing the results data as well as any required
            //        //   metadata that is used by plugins. The object should contain an array of
            //        //   data objects as the `results` key.
            //        processResults: function (data) {
            //            var proj = $.grep(data, function(item) {
            //                return item.ObjCode === 'PROJ';
            //            });
            //            var task = $.grep(data, function (item) {
            //                return item.ObjCode === 'TASK';
            //            });
            //            var issue = $.grep(data, function (item) {
            //                return item.ObjCode === 'OPTASK';
            //            });

            //            var res = [];

            //            if (proj.length > 0) {
            //                res.push({
            //                    id: '1',
            //                    text: 'Projects',
            //                    children: $.map(proj, function(item, index) {
            //                        return {
            //                            id: item.ID,
            //                            text: item.Name
            //                        };
            //                    })
            //                });
            //            }

            //            if (task.length > 0) {
            //                res.push({
            //                    id: '2',
            //                    text: 'Tasks',
            //                    children: $.map(task, function(item, index) {
            //                        return {
            //                            id: item.ID,
            //                            text: item.Name
            //                        };
            //                    })
            //                });
            //            }

            //            if (issue.length > 0) {
            //                res.push({
            //                    id: '2',
            //                    text: 'Issues',
            //                    children: $.map(issue, function(item, index) {
            //                        return {
            //                            id: item.ID,
            //                            text: item.Name
            //                        };
            //                    })
            //                });
            //            }

            //            return { results: res };
            //        },
            //        // You can use a custom AJAX transport function if you do not want to use the
            //        // default one provided by jQuery.
            //        //
            //        // @param params The object containing the parameters used to generate the
            //        //   request.
            //        // @param success A callback function that takes `data`, the results from the
            //        //   request.
            //        // @param failure A callback function that indicates that the request could
            //        //   not be completed.
            //        // @returns An object that has an `abort` function that can be called to abort
            //        //   the request if needed.
            //        transport: function(params, success, failure) {
            //            var $request = $.post(params.url, { '': params.data });

            //            $request.then(success);
            //            $request.fail(failure);

            //            return $request;
            //        }
            //    }


            //    //data: data
            //    //templateSelection: function(item) {
            //    //    return item.text;
            //    //},
            //    //templateResult: function(item) {
            //    //    return $('<span class="truncate">' + item.text + '</span>');
            //    //}
            //});

            setHost();
            $('body').append($('<div class="loading"></div>').hide());
        });

        $("#send").click(doLogin);
        $("#mywork").click(doMyWork);
        $("#objects").click(doGetObjects);
        $("#handlers").click(doGetAttachmentsHandles);


    };
})();