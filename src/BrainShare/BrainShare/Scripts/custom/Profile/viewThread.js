var MessagesModel = function (data) {

    var self = this;

    this.messages = ko.observableArray(data.Messages);
    this.threadId = data.ThreadId;
    this.content = ko.observable();


    this.submitMessage = function () {

        var formData = { threadId: self.threadId, content: self.content() };
        send("/profile/thread/post", formData, function (message) {
            self.content("");
            self.messages.push(message);
        });

    };
}


