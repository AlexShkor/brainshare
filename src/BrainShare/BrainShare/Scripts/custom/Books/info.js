var InfoModel = function (data) {
    var self = this;
    this.book = data;
    
    this.take = function() {
        send("/books/take", self.book, function (response) {
            window.location = "/books/take/" + response.Id;
        });
    };

    this.give = function() {
        send("/books/give", self.book, function (response) {

        });
    };
}