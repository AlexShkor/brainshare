var EditBookViewModel = function(data, submitUrl) {
    var self = this;
    this.model = ko.mapping.fromJS(data);

    this.save = function() {
        sendModel(submitUrl, self.model, function () {
           // window.location = "/books/info"
        },["Languages"]);
    };

    this.addISBN = function() {
        self.model.ISBNs.push({
            Value: ko.observable()
        });
    };
    this.addAuthor = function() {
        self.model.Authors.push({
            Value: ko.observable()
        });
    };
    this.removeAuthor = function (author) {
        if (self.model.Authors().length > 1) {
            self.model.Authors.remove(author);
        }
    };
    this.removeISBN = function (isbn) {
        if (self.model.ISBNs().length > 1) {
            self.model.ISBNs.remove(isbn);
        }
    };

    this.toMine = function() {
        self.model.IsWhishBook(false);
    };
    this.toWish = function() {
        self.model.IsWhishBook(true);
    };
}; 