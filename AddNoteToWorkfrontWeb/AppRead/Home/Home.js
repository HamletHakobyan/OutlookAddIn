/// <reference path="../App.js" />

(function () {
    "use strict";

    function getHost() {
        var mailbox = Office.context.mailbox;
        var item = mailbox.item;
        var matches = item.getRegExMatchesByName("CommentOnProject");
        var match = matches[0];
        return match;
    }

    function setHost() {
        var host = getHost();
        $('#url').val(host);
    }

    // The Office initialize function must be run each time a new page is loaded
    Office.initialize = function (reason) {
        $(document).ready(function () {
            setHost();
        });
    };
})();