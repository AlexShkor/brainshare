var TakeModel = function (data) {
    var self = this;
    this.owners = ko.observableArray(data.Owners);
    this.sendRequest = function (owner) {
        window.location = "/books/take/" + self.book.Id + "/from/" + owner.UserId;
    };

    // ViewModel for PartialView
    this.book = data.Book;
}