var MessagesModel = function (data) {

    var self = this;

    this.userId = data.UserId;
    this.messages = ko.observableArray(data.Messages);
    this.threadId = data.ThreadId;
    this.content = ko.observable();

    function isNullOrWhiteSpace(str) {
        if (str === undefined) {
            return false;
        }
        return str === null || str.match(/^\s*$/) !== null;
    };
    
    this.submitOnEnter = function (dat, event) {
        // var test = self.content();  === undefined  without jQuery function 
        if (event.ctrlKey && event.keyCode == 13) {
            self.submitMessage();
            self.content("");
            return false;
        }

        //var test2 = self.content();  === undefined  without jQuery function 
        var message = $("#content").val();
        self.content(message);
        return false;
    };

    this.submitMessage = function () {

        if (isNullOrWhiteSpace(self.content())) {
            self.content("");
            return false;
        }
        
        var formData = { threadId: self.threadId, content: self.content() };
        send("/profile/thread/post", formData, function (message) {
            self.content("");
            self.messages.unshift(message);
            $("abbr.timeago").timeago();
        });
        return false;
    };

    this.initServerCallback = function () {

        var threadHub = $.connection.threadHub;

        threadHub.client.messageSent = function (e) {
            if (e.UserId != self.userId) {
                self.messages.unshift(e);
            }
        };
        threadHub.server.join(self.threadId);
    };
};