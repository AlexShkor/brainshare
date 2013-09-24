var TakeModel = function (data) {
    var self = this;
    this.owners = ko.observableArray(data.Owners);
    this.sendRequest = function (owner) {
        window.location = "/books/take/" + owner.BookId + "/from/" + owner.UserId;
    };
    this.seeProfile = function(owner) {
        window.location = "/profile/view/" + owner.UserId;
    };

    // ViewModel for PartialView
    this.book = data.Book;
}