
var AddBookModel = function(ownedItems) {
    var self = this;

    this.postfix = "&key=AIzaSyAFFukdkjHMHh5WmucwuxVGt18XA9LEJ1I&language=ru&country=";
    this.baseUrl = "https://www.googleapis.com/books/v1/volumes?q=";

    this.query = ko.observable();
    this.loading = ko.observable(false);
    this.selectedLanguage = ko.observable("RU");
   

    this.availableLanguages = ko.observableArray(
       [ { title: "Русский", code: "RU" },
        { title: "Английский", code: "US" }]
    );
    
    this.items = ko.observableArray();
    
    this.owned = ko.observableArray(ownedItems);

    this.search = function () {
        if (self.query()) {
            self.loading(true);
            $.getJSON(self.baseUrl + encodeURIComponent(self.query()) + self.postfix + self.selectedLanguage(),
                function(response) {
                    if (response.items) {
                        self.items.removeAll();
                        $.each(response.items, function(index, item) {
                            self.items.push(new BookViewModel(item, self));
                        });
                    }
                    self.loading(false);
                });
        }
    };

    this.languageChanged = function(item) {
        setTimeout(function() {
            self.search();
        }, 10);
    };

    this.give = function (item) {
        self.owned.push(item.GoogleBookId);
        send("/books/give", item, function (response) {
           
        });
    };


    this.info = function (item) {
        var callback = function(response) {
            window.location = "/books/info/" + response.Id;
        };
        send("/books/info", item, callback );
    };


    this.take = function (item) {
        
        send("/books/take", item, function (response) {
            window.location = "/books/take/" + response.Id;
        });
    };

};


var BookViewModel = function(item, parent) {
    var self = this;
    this.GoogleBookId = item.id;
    this.Country = parent.selectedLanguage();
    this.SearchInfo = (item.searchInfo || { }).textSnippet;
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
        $.each(isbns, function(index, isbn) {
            self.ISBNS.push(isbn.identifier);
        });
    }
};
