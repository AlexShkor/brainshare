﻿
var AddBookModel = function (model) {
    var self = this;

    this.postfix = "&key=AIzaSyAFFukdkjHMHh5WmucwuxVGt18XA9LEJ1I&maxResults=20&country=ru";
    this.baseUrl = "https://www.googleapis.com/books/v1/volumes?q=";

    this.query = ko.observable();
    this.loading = ko.observable(false);
    this.selectedLanguage = ko.observable(null);


    this.availableLanguages = ko.observableArray(
       [
           { title: "Любой", code: null },
           { title: "Русский", code: "ru" },
           { title: "Английский", code: "en" }]
    );

    this.items = ko.observableArray();

    this.totalItems = ko.observable(null);

    this.pages = ko.observableArray();

    this.ownedBooks = ko.observableArray(model.MyBooksIds);
    
    this.wishedBooks = ko.observableArray(model.WishBooksIds);

    this.startIndex = ko.observable(0);

    this.submit = function() {
        self.startIndex(0);
        self.search();
    };

    this.search = function () {
        if (self.query()) {
            self.loading(true);
            $.getJSON(self.baseUrl + encodeURIComponent(self.query()) + self.postfix + (self.selectedLanguage() ? "&langRestrict=" + self.selectedLanguage() : "") + "&startIndex=" + self.startIndex(),
                function (response) {
                    if (response.items) {
                        self.items.removeAll();
                        $.each(response.items, function (index, item) {
                            self.items.push(new BookViewModel(item, self));
                        });
                    }
                    self.loading(false);
                    self.pages.removeAll();
                    for (var i = 0; i < response.totalItems/20; i++) {
                        self.pages.push({
                            Number: ko.observable(i + 1),
                            StartIndex: ko.observable(i*20)
                        });
                    }
                });
        }
    };

    this.languageChanged = function (item) {
        setTimeout(function () {
            self.search();
        }, 10);
    };

  
    this.hideConfirmBtns = function (googleBookId) {
        $("#confirmBtns-" + googleBookId).hide();
        $("#wishBtns-" + googleBookId).fadeIn();
    };

    this.goToPage = function(page) {
        self.startIndex(page.StartIndex());
        self.search();
    };

    this.give = function (item) {
  
        send("/books/give", item, function (response) {
            self.ownedBooks.push(item.GoogleBookId);
        });
    };

    this.take = function (item) {

        send("/books/take", item, function (response) {
            window.location = "/Profile/WishList";
        });
    };

};


var BookViewModel = function (item, parent) {
    var self = this;
    this.GoogleBookId = item.id;
    this.Country = parent.selectedLanguage();
    this.SearchInfo = (item.searchInfo || {}).textSnippet;
    this.Authors = ko.observableArray(item.volumeInfo.authors);
    this.Categories = ko.observableArray(item.volumeInfo.categories);
    this.Language = item.volumeInfo.language;
    this.PageCount = item.volumeInfo.pageCount;
    this.PublishedDate = item.volumeInfo.publishedDate;
    this.Publisher = item.volumeInfo.publisher;
    this.Subtitle = item.volumeInfo.subtitle;
    this.Title = item.volumeInfo.title;
    this.Image = (item.volumeInfo.imageLinks || {}).thumbnail;
    var isbns = item.volumeInfo.industryIdentifiers;
    if (isbns) {
        this.ISBNS = ko.observableArray();
        $.each(isbns, function (index, isbn) {
            self.ISBNS.push(isbn.identifier);
        });
    }
};
