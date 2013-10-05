var AllUsersModel = function (data) {
    var self = this;
    this.items = ko.mapping.fromJS(data);

    this.info = function(item) {
        window.location = "/profile/view/" + item.UserId();
    };
}; 