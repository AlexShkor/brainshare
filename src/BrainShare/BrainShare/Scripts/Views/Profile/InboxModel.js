﻿var InboxModel = function (data) {

    var self = this;

    this.items = ko.observableArray(data.Items);

    this.consider = function (item) {
        window.location = "/books/consider/" + item.BookId + "/from/" + item.User.UserId;
    };
};