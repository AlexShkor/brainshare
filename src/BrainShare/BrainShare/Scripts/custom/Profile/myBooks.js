var MyBooksModel = function (data) {

    var self = this;

    this.books = ko.observableArray(data);

    this.dontHave = function (book) {
        send("/Profile/DontHave", book, function (response) {
            self.books.remove(book);
        });
    };
};