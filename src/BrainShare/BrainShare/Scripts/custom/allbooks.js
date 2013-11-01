
var AllBooksModel = function (data) {
    var self = this;

    this.languages = ko.observableArray(data.Languages);

    delete data.Languages;

    this.filter = ko.mapping.fromJS(data);

    ApplyFilterModel(self, "/books");

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
