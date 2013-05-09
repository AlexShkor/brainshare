var WishListModel = function(data) {

    var self = this;

    this.books = ko.observableArray(data);

    this.whoHas = function(book) {
        
        window.location = "/books/take/" + book.Id;
       
    };
    
    this.dontWant = function (book) {
        send("/profile/dontwant", book, function (response) {
            self.books.remove(book);
        });
    };
};

