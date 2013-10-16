﻿var AllUsersModel = function (data) {
    var self = this;
    this.items = ko.mapping.fromJS(data);

    this.info = function(item) {
        window.location = "/profile/view/" + item.UserId();
    };

    this.CutString = function (string, limit) {
        if (string.length > limit) {
            var cutString = string.substring(0, limit);
            return cutString + "...";
        }
        return string;
    };
}; 