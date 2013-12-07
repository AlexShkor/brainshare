var AllUsersModel = function (data) {
    console.log(data);
    var self = this;
    //this.items = ko.mapping.fromJS(data.Users);
    
    delete data.Users;

    this.filter = ko.mapping.fromJS(data);
    
    ApplyFilterModel(self, "/users");

    console.log(ko.mapping.toJS(self));

    this.info = function(item) {
        window.location = "/profile/view/" + item.UserId();
    };

    this.CutString = function (string, limit) {
        if (string != null && string.length > limit) {
            var cutString = string.substring(0, limit);
            return cutString + "...";
        }
        return string;
    };
}; 