var InfoModel = function (data) {
    var self = this;
    this.book = data;
    this.comments = ko.observableArray();

    this.commentText = ko.observable("");

    this.replyText = ko.observable("");

    this.replyId = ko.observable(null);

    this.take = function () {
        send("/books/take", self.book, function (response) {
            window.location = "/books/take/" + response.Id;
        });
    };

    this.give = function () {
        send("/books/give", self.book, function (response) {

        });
    };

    this.reply = function(comment) {
        self.replyId(comment.Id());
    };

    this.sendReply = function (comment) {
        if (!self.replyText()) {
            return;
        }
        sendJson("/comments/addreply", { id: self.book.Id, commentId: comment.Id(), content: self.replyText() }, function (response) {
            comment.Replies.push(response);
            self.replyId(null);
            self.replyText("");
        });
    };

    this.sendComment = function() {
        sendJson("/comments/addcomment", { id: self.book.Id, content: self.commentText() }, function (response) {
            self.comments.unshift(ko.mapping.fromJS(response));
            self.commentText(null);
        });
    };

    sendJson("/comments/load", { id: data.Id }, function (response) {
        for (var i = 0; i < response.length; i++) {
            self.comments.push(ko.mapping.fromJS(response[i]));
        }
    });
}