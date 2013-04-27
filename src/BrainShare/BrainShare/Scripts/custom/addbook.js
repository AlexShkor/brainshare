
var AddBookModel = function() {
    var self = this;

    this.postfix = "&key=AIzaSyAFFukdkjHMHh5WmucwuxVGt18XA9LEJ1I&language=ru&country=";
    this.baseUrl = "https://www.googleapis.com/books/v1/volumes?q=";

    this.query = ko.observable();
    this.selectedLanguage = ko.observable();
   

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

    this.search = function() {
        $.get(self.baseUrl + encodeURIComponent(self.query()) + self.postfix + self.selectedLanguage(),

            function (response) {

                self.items.removeAll();
                $.each( response.items,function(index,item) {
                    self.items.push(item);
                });
            });
    };

    this.give = function() {

    };

    this.take = function() {

    };

};
