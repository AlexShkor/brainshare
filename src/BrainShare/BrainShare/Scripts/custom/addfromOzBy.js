
var AddFromOzByBookModel = function () {
    var self = this;

    this.query = ko.observable();
    
    this.loading = ko.observable(false);
    
    this.items = ko.observableArray();

    this.visibleItemsPerPage = 10;
    
    this.visibleItems = ko.observableArray();

    this.pages = ko.observableArray();
    
    this.page = ko.observable(1);

    this.submit = function() {
        self.page(1);
        self.pages.removeAll();
        self.items.removeAll();
        self.visibleItems.removeAll();
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
                        if (self.visibleItemsPerPage > j) {
                            self.visibleItems.push(s);
                        }
                    }
                    self.loading(false);
                    self.pages.removeAll();
                    for (var i = 1; i < self.items().length / self.visibleItemsPerPage; i++) {
                        self.pages.push({
                            Number: ko.observable(i),
                            StartIndex: ko.observable((i-1)*20)
                        });
                    }
                });
        }
    };

    this.copyVisibleItems = function(start,length) {
        self.visibleItems.removeAll();
        
        for (var i = 0; i < length; i++) {
            var index = start + i;
            if (index >= self.items().length) {
                return;
            }
            self.visibleItems.push(self.items()[index]);
        }
    };

    this.goToPage = function (page) {
        var startIndex = page.StartIndex();
        self.copyVisibleItems(startIndex, self.visibleItemsPerPage);
        self.page(page.Number());
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
