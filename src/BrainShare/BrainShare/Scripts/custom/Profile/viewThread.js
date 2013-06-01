var MessagesModel = function(data) {

    var self = this;

    this.userId = data.UserId;
    this.messages = ko.observableArray(data.Messages);
    this.threadId = data.ThreadId;
    this.content = ko.observable();


    this.submitMessage = function() {

        var formData = { threadId: self.threadId, content: self.content() };
        send("/profile/thread/post", formData, function(message) {
            self.content("");
            self.messages.unshift(message);
        });

    };

    this.initServerCallback = function() {

        var threadHub = $.connection.threadHub;

        threadHub.client.messageSent = function(e) {
            if (e.UserId != self.userId) {
                self.messages.unshift(e);
            }
        };

        $.connection.hub.start(function() {
            threadHub.server.join(self.threadId);
        });
    };
};
