var AllMessagesModel = function (data) {

    var self = this;

    this.items = ko.observableArray(data.Items);

    this.reply = function(item) {
        window.location = "/Profile/Thread/View/" + item.ThreadId;
    };
}