var AllUsersModel = function (data) {
    var self = this;

    this.filter = ko.mapping.fromJS(data);
    
    ApplyFilterModel(self, "/users");

    this.info = function (item) {
        console.log(item);
        window.location = "/profile/view/" + item.UserId();
    };

    this.CutString = function (string, limit) {
        if (string != null && string.length > limit) {
            var cutString = string.substring(0, limit);
            return cutString + "...";
        }
        return string;
    };
    
    this.toggleAdvancedSearch = function () {
        self.filter.Advanced(!self.filter.Advanced());
    };
}; 