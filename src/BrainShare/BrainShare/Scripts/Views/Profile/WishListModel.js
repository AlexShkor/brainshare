var WishListModel = function(data) {

    var self = this;

    this.books = ko.observableArray(data);

    this.whoHas = function(book) {
        
        window.location = "/Books/Take/" + book.Id;
       
    };
    
    this.dontWant = function (book) {
        send("/Profile/DontWant", book, function (response) {
            self.books.remove(book);
        });
    };
};

