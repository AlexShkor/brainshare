
var AddFromOzByBookModel = function () {
    var self = this;

    this.query = ko.observable();
    
    this.loading = ko.observable(false);
    
    this.items = ko.observableArray();

    this.visibleItemsPerPage = 10;
    
    this.visibleItems = ko.observableArray();

    this.pages = ko.observableArray();
    
    this.page = ko.observable(1);

    this.pageRange = ko.observableArray([10, 25, 50]);

    this.selectedRange = ko.observable(25);
    this.selectedRange.subscribe(function (newVal) {
        self.visibleItemsPerPage = newVal;
        self.submit();
    });
    

    this.startIndex = ko.observable(0);

    this.submit = function () {
        this.startIndex(0);
        self.items.removeAll();
        self.pages.removeAll();
        self.search();
    };

    this.search = function () {
        if (self.query()) {
            self.loading(true);
            $.get("/books/downloadstring?q=" + encodeURIComponent(self.query()) + "&page=" + self.page() + "&&searchLimit=" + self.visibleItemsPerPage,
                function (response) {
                    self.items.removeAll();
                    var ozPages = $(response).find(".pages > .nolist .longcol").children();
                    var lastPage = parseInt($(ozPages).last().text());
 
                    var elements = $(response).find("ul.big-thumb-listing > li");
                    for (var j = 0; j < elements.length; j++) {
                        var el = $(elements[j]);
                        var arr = el.find("div.f11").text().trim().split(", ");
                        var yaer = arr.pop();

                        var s = {
                            Title: el.find("h2 a").text(),
                            Image: el.find("img").attr("src").replace("listing.ozstatic.by/70", "goods.ozstatic.by/200"),
                            Description: $(el.find("p")[1]).text(),
                            Authors: ko.observableArray(arr),
                            Year: yaer,
                            Added: ko.observable(false) 
                        };
                        self.items.push(s);
                    }
                    self.loading(false);
           
                    self.pages.removeAll();
                    for (var i = 0; i < lastPage ; i++) {
                        self.pages.push({
                            Number: ko.observable(i + 1),
                            StartIndex: ko.observable(i * self.visibleItemsPerPage)
                        });
                    }
                });
        }
    };


    this.goToPage = function (page) {
        self.page(page.Number());
        self.search();       
    };

    this.give = function (item) {
        send("/books/my/add/from-oz", item, function(response) {
            item.Added(true);
        });
    };

    this.take = function (item) {

        send("/books/wish/add/from-oz", item, function (response) {
            item.Added(true);
        });
    };

};
