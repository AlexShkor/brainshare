var InboxModel = function (data) {

    var self = this;

    this.items = ko.observableArray(data.Items);

    this.consider = function (item) {
        window.location = "/Books/Accept/" + item.Book.Id + "/From/" + item.User.UserId;
    };
};