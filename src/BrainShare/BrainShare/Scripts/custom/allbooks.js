﻿
var AllBooksModel = function (data) {
    var self = this;

    this.filter = ko.mapping.fromJS(data);

    ApplyFilterModel(self, "/books");

    this.info = function (item) {
        window.location = "/books/info/" + item.Id();
    };

    this.CutString = function (string, limit) {
        if (string != null && string.length > limit) {
            var cutString = string.substring(0, limit);
            return cutString + "...";
        }
        return string;
    };
};
