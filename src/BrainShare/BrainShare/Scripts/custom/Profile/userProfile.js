var UserProfileModel = function (data) {
    var self = this;
    this.user = data;

    this.notSubscribedTemplateName = "subscribe-template";
    this.subscribedTemplateName = "subscribed-template";

    // Reputation section
    this.canIncrease = ko.observable(self.user.CanIncrease);
    this.canDecrease = ko.observable(self.user.CanDecrease);
    this.summaryVotes = ko.observable(self.user.SummaryVotes);
    this.isCurrentUserSubscribed = ko.observable(self.user.IsCurrentUserSubscribed);
    this.status = ko.observable(self.user.Status);
    console.log(data);
    
    this.templateName = ko.computed(function() {
        return self.isCurrentUserSubscribed() ? self.subscribedTemplateName : self.notSubscribedTemplateName;
    });
    this.increaseReputation = function () {
        var parameters = { id: self.user.Id, value: 1 };
        send("/Profile/AdjustReputation", parameters, function (response) {
            self.summaryVotes(response.summaryVotes);
            self.canIncrease(response.canIncrease);
            self.canDecrease(response.canDecrease);
        });
    };
    this.decreaseReputation = function () {
        var parameters = { id: self.user.Id, value: -1 };
        send("/Profile/AdjustReputation", parameters, function (response) {
            self.summaryVotes(response.summaryVotes);
            self.canIncrease(response.canIncrease);
            self.canDecrease(response.canDecrease);
        });
    };

    this.listOfOwnedBooks = ko.observableArray();
    this.listOfWishedBooks = ko.observableArray();

    function getOwnedBooks() {
        self.listOfOwnedBooks.removeAll();
        send("/profile/get-user-books", { userId: self.user.Id }, function (response) {
            $.each(response, function (index, book) {
                self.listOfOwnedBooks.push(book);
            });
        });
    };
    
    function getWishedBooks() {
        self.listOfWishedBooks.removeAll();
        send("/profile/get-user-wish-books", { userId: self.user.Id }, function (response) {
            $.each(response, function (index, book) {
                self.listOfWishedBooks.push(book);
            });
        });
    };

    this.sendMessage = function () {
        window.location = "/profile/message/to/" + self.user.Id;
    };

    this.subscribe = function () {
        send("/Follower/Subscribe", { publisherId: self.user.Id }, function (response) {
            if (response == "success") {
                self.isCurrentUserSubscribed(true);
            }
        });
    };
    
    this.unsubscribe = function () {
        send("/Follower/Unsubscribe", { userId: self.user.Id }, function (response) {
            if (response == "success") {
                self.isCurrentUserSubscribed(false);
            }
        });
    };

    this.viewWishBook = function(book) {
        window.location = "/books/info/wish/" + book.Id;
    };
    
    this.viewBook = function(book) {
        window.location = "/books/info/" + book.Id;
    };

    getOwnedBooks();
    getWishedBooks();
};