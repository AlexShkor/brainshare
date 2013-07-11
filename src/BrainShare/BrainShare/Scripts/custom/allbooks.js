
var AllBooksModel = function(data) {
    var self = this;

    
    
    this.items = ko.mapping.fromJS(data.Items);

    this.owned = ko.observableArray(data.OwnedItems);

    this.give = function (item) {
        send("/books/give", item, function (response) {
            self.owned.push(response.Id); 
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
