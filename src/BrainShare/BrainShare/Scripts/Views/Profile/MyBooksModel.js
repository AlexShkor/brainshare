var MyBooksModel = function (data) {

    var self = this;

    this.books = ko.mapping.fromJS(data);

    this.dontHave = function (book) {
        send("/Profile/DontHave", book, function (response) {
            self.books.remove(book);
        });
    };

    //todo: handle errors with tooltips and rollback status
    this.UpdateReadableStatus = function(book) {
        send("/Profile/UpdateBookStatus", { id: book.Id() }, function (response) {
        });
        return true;
    };
    

};