var WishListModel = function(data) {

    var self = this;

    this.books = ko.observableArray(data);

    this.whoHas = function(book) {
        
        window.location = "/Books/Index#search/Search=" + encodeURIComponent(encodeURIComponent(book.Title));
       
    };
    
    this.dontWant = function (book) {
        send("/Profile/DontWant", book, function (response) {
            self.books.remove(book);
        });
    };
};

