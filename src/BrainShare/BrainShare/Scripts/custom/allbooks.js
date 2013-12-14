
var AllBooksModel = function (data) {
    var self = this;

    this.languages = ko.observableArray(data.Languages);

    delete data.Languages;

    this.filter = ko.mapping.fromJS(data);
   

    ApplyFilterModel(self, "/books");

    if (data.Search) {
        console.log(data);
        self.filter.Search(data.Search);
        self.search();
        self.filter.ISBN(null);
    }
    
    if (data.ISBN) {
        console.log(data);
        self.filter.ISBN(data.ISBN);
        
        self.successFilterCallback = function() {
            self.filter.ISBN(null);
        };
        self.search();
        console.log(ko.mapping.toJS(self.filter));
    }
    
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

    this.toggleAdvancedSearch = function() {
        self.filter.Advanced(!self.filter.Advanced());
    };
};
