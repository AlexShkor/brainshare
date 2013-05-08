
var AddBookModel = function(ownedItems) {
    var self = this;

    this.postfix = "&key=AIzaSyAFFukdkjHMHh5WmucwuxVGt18XA9LEJ1I&language=ru&country=";
    this.baseUrl = "https://www.googleapis.com/books/v1/volumes?q=";

    this.query = ko.observable();
    this.loading = ko.observable(false);
    this.selectedLanguage = ko.observable("RU");
   

    this.availableLanguages = ko.observableArray(
       [ { title: "Русский", code: "RU" },
        { title: "Анлглийский", code: "US" }]
    );
    
    //this.searchType = ko.observable();
    //this.searchTypes = ko.observableArray(
    //   [ 
    //       { title: "Везде", code: "" },
    //       { title: "В названии", code: "+intitle" },
    //       { title: "В авторах", code: "+inauthor" },
    //       { title: "В издателе", code: "+inpublisher" },
    //       { title: "В категориях", code: "+subject" },
    //       { title: "ISBN", code: "+isbn" }
    //   ]
    //);
    
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
                            self.items.push(new BookViewModel(item));
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

        var data = self.mapData(item);
        $.post("/books/give", ko.toJS(data), function (response) {
            if (response.Error) {
                alert(response.Error);
            } else {
                self.owned.push(response.Id);
            }
        });
    };

    this.mapData = function(item) {
        var data = {
            Id: item.id,
            Country: self.selectedLanguage(),
            SearchInfo: (item.searchInfo || { }).textSnippet,
            Authors: new Array(item.volumeInfo.authors),
            Categories: new Array(item.volumeInfo.categories),
            Language: item.volumeInfo.language,
            PageCount: item.volumeInfo.pageCount,
            PublishedDate: item.volumeInfo.publishedDate,
            Publisher: item.volumeInfo.publisher,
            Subtitle: item.volumeInfo.subtitle,
            Title: item.volumeInfo.title,
            Image: (item.volumeInfo.imageLinks || { }).thumbnail,
            ISBN: item.volumeInfo.industryIdentifiers[0].identifier
        };
        return data;
    };

    this.take = function (item) {

        var data = self.mapData(item);
        $.ajax({
            type: "POST",
            url: "/books/take",
            data: { book: JSON.stringify(data) },
            success: function (response) {
                if (response.Error) {
                    alert(response.Error);
                } else {
                    window.location = "/books/take/" + response.Id;
                }
            }
        });
    };

};


var BookViewModel = function(data) {
    var self = this;
    this.Id = item.id;
    this.Country = self.selectedLanguage();
    this.SearchInfo = (item.searchInfo || { }).textSnippet;
    this.Authors = ko.observableArray(item.volumeInfo.authors);
    this.Categories = ko.observableArray(item.volumeInfo.categories);
    this.Language = item.volumeInfo.language;
    this.PageCount = item.volumeInfo.pageCount;
    this.PublishedDate = item.volumeInfo.publishedDate;
    this.Publisher = item.volumeInfo.publisher;
    this.Subtitle = item.volumeInfo.subtitle;
    this.Title = item.volumeInfo.title;
    this.Image = (item.volumeInfo.imageLinks || { }).thumbnail;
    this.ISBN = item.volumeInfo.industryIdentifiers[0].identifier;
}