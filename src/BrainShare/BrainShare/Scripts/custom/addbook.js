
var AddBookModel = function() {
    var self = this;

    this.postfix = "&key=AIzaSyAFFukdkjHMHh5WmucwuxVGt18XA9LEJ1I&country=";
    this.baseUrl = "https://www.googleapis.com/books/v1/volumes?q=";

    this.query = ko.observable();
    this.selectedLanguage = ko.observable();

    this.availableLanguages = ko.observableArray(
       [ { title: "Русский", code: "RU" },
        { title: "Анлглийский", code: "US" }]
    );
    this.items = ko.observableArray();

    this.search = function() {
        $.get(self.baseUrl + self.query() + self.postfix + self.selectedLanguage(),

            function (response) {

                self.items.removeAll();
                $.each( response.items,function(index,item) {
                    self.items.push(item);
                });
            });
    };

};
