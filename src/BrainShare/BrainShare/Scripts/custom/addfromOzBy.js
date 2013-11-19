
var AddFromOzByBookModel = function (ownedItems) {
    var self = this;
    
    this.baseUrl = "http://oz.by/search/?catalog_id=1101523&q=";

    this.query = ko.observable();
    
    this.loading = ko.observable(false);
    
    this.items = ko.observableArray();

    this.pages = ko.observableArray();
    
    this.page = ko.observable(1);

    this.owned = ko.observableArray(ownedItems);
    

    this.submit = function() {
        this.page(1);
        self.search();
    };

    this.search = function () {
        if (self.query()) {
            self.loading(true);
            $.get("/books/downloadstring?q=" + encodeURIComponent(self.query()) + "&page=" + self.page(),
                function (response) {
                    self.items.removeAll();

                    var elements = $(response).find("ul.big-thumb-listing > li");
                    for (var j = 0; j < elements.length; j++) {
                        var el = $(elements[j]);
                        var arr = el.find("div.f11").text().split(", ");
                        var yaer = arr.pop();

                        var s = {
                            Title: el.find("h2 a").text(),
                            Image: el.find("img").attr("src"),
                            SearchInfo: $(el.find("p")[1]).text(),
                            Authors: ko.observableArray(arr),
                            PublishedDate: yaer
                        };
                        self.items.push(s);
                    }
                    self.loading(false);
                    //self.pages.removeAll();
                    //for (var i = 0; i < response.totalItems/20; i++) {
                    //    self.pages.push({
                    //        Number: ko.observable(i + 1),
                    //        StartIndex: ko.observable(i*20)
                    //    });
                    //}
                });
            
            //$.ajax({
            //    dataType: "jsonp",
            //    url: self.baseUrl + encodeURIComponent(self.query()) + self.postfix,
            //    crossDomain: true,
            //    success: function (data) {
            //        if (response.items) {
            //            self.items.removeAll();
            //            $.each(response.items, function (index, item) {
            //                self.items.push(new BookViewModel(item, self));
            //            });
            //        }
            //        self.loading(false);
            //    },
            //    error: function () {
            //        console.log(arguments)
            //    }
            //});
        }
    };

  
    this.hideConfirmBtns = function (googleBookId) {
        $("#confirmBtns-" + googleBookId).hide();
        $("#wishBtns-" + googleBookId).fadeIn();
    };

    this.goToPage = function(page) {
        self.startIndex(page);
        self.search();
    };

    this.give = function (item) {
        debugger;
        self.owned.push(item.GoogleBookId);
       
        //var data = {
        //    bookDto: ko.toJS(item),
        //    shareOnFb: shareOnFb
        //};


        //$.ajax({
        //    type: "POST",
        //    url: "/books/give",
        //    contentType: "application/json; charset=utf-8",
        //    data: data,
        //    dataType: "json",
        //    success: function(response) {

        //    }
        //});

        send("/books/give", item, function (response) {

        });
    };


    this.info = function (item) {
        var callback = function (response) {
            window.location = "/books/info/" + response.Id;
        };
        send("/books/info", item, callback);
    };


    this.take = function (item) {

        send("/books/take", item, function (response) {
            window.location = "/books/take/" + response.Id;
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
