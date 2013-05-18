﻿var UserProfileModel = function (data) {
    var self = this;
    this.user = data;

    this.showOwnedBooks = ko.observable(false);
    this.showWishedBooks = ko.observable(false);

    this.showOwnedBooksButton = ko.observable(true);
    this.showWishedBooksButton = ko.observable(true);

    this.hideOwnedBooks = function () {
        self.showOwnedBooks(false);
        self.showOwnedBooksButton(true);
    };
    this.hideWishedBooks = function () {
        self.showWishedBooks(false);
        self.showWishedBooksButton(true);
    };

    this.listOfOwnedBooks = ko.observableArray();
    this.listOfWishedBooks = ko.observableArray();

    this.getOwnedBooks = function () {
        self.listOfOwnedBooks.removeAll();
        var ids = { ids: self.user.Books };
        send("/Profile/getBooks", ids, function (response) {
            $.each(response.books, function (index, book) {
                self.listOfOwnedBooks.push(book);
            });
        });
        self.showOwnedBooks(true);
        self.showOwnedBooksButton(false);
    };
    this.getWishedBooks = function () {
        self.listOfWishedBooks.removeAll();
        var ids = { ids: self.user.WishList };
        send("/Profile/getBooks", ids, function (response) {
            $.each(response.books, function (index, book) {
                self.listOfWishedBooks.push(book);
            });
        });
        self.showWishedBooks(true);
        self.showWishedBooksButton(false);
    };

    this.sendMessage = function () {
        window.location = "/profile/message/to/" + self.user.Id;
    };
};


var UserBookViewModel = function (item) {
    this.SearchInfo = (item.searchInfo || {}).textSnippet;
    this.Authors = item.volumeInfo.authors;
    this.Categories = item.volumeInfo.categories;
    this.Language = item.volumeInfo.language;
    this.PageCount = item.volumeInfo.pageCount;
    this.PublishedDate = item.volumeInfo.publishedDate;
    this.Publisher = item.volumeInfo.publisher;
    this.Subtitle = item.volumeInfo.subtitle;
    this.Title = item.volumeInfo.title;
    this.Image = (item.volumeInfo.imageLinks || {}).thumbnail;
    this.ISBN = item.volumeInfo.industryIdentifiers[0].identifier;
};
