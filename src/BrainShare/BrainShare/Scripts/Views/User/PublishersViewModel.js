function PublishersViewModel(data) {
    var self = this;

    this.publisherTemplate = data.PublisherTemplate;
    this.publisherDeletedTemplate = data.PublisherDeletedTemplate;

    this.model = ko.mapping.fromJS(data);


    this.showProfile = function (publisher) {
        document.location = "/profile/view/" + publisher.Id();
    };

    this.showPublishers = function (publisher) {
        document.location = "/User/Friends/" + publisher.Id();
    };

    this.sendMessage = function (publisher) {
        document.location = "/profile/message/to/" + publisher.Id();
    };

    this.unsubscribe = function (publisher) {
        send("/Follower/Unsubscribe", { userId: publisher.Id() }, function (response) {
            publisher.TemplateName(self.publisherDeletedTemplate);
        });
    };

    this.subscribe = function (publisher) {
        send("/Follower/Subscribe", { publisherId: publisher.Id() }, function (response) {
            publisher.TemplateName(self.publisherTemplate);
        });
    };

}
