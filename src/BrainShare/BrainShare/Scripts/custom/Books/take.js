var TakeModel = function (data) {
    var self = this;
    this.book = data;
    this.owners = ko.observableArray(data.Owners);
    this.sendRequest = function (owner) {
        window.location = "/books/take/" + self.book.Id + "/from/" + owner.UserId;
    };
}