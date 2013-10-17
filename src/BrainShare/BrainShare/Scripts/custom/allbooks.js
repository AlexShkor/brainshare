
var AllBooksModel = function (data) {
    var self = this;

    this.items = ko.mapping.fromJS(data.Items);
    this.owned = ko.observableArray(data.OwnedItems);

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
};
