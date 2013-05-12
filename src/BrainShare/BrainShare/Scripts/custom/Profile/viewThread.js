var MessagesModel = function (data) {

    var self = this;

    this.messages = ko.observableArray(data.Messages);
    this.threadId = data.ThreadId;

    this.submitMessage = function () {

        var threadId = $("#threadId").val();
        var content = $("#content").val();

        var formData = { threadId: threadId, content: content };
        send("/profile/thread/post", formData, function (message) {
            $("#content").val("");
            self.messages.push(message);
        });

    };
}


