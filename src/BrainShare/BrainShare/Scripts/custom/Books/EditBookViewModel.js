﻿var EditBookViewModel = function(data) {
    var self = this;
    this.model = ko.mapping.fromJS(data);

    this.save = function() {

    };

    this.addISBN = function() {
        self.model.ISBNs.push("");
    };
    this.addAuthor = function() {
        self.model.Authors.push("");
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
}; 