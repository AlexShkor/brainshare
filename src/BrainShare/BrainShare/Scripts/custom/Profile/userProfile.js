var UserProfileModel = function (data) {
    var self = this;
    this.user = data;

    // Reputation section
    this.canIncrease = ko.observable(self.user.CanIncrease);
    this.canDecrease = ko.observable(self.user.CanDecrease);
    this.summaryVotes = ko.observable(self.user.SummaryVotes);
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


    this.showOwnedBooks = ko.observable(false);
    this.showWishedBooks = ko.observable(false);

    this.showOwnedBooksButton = ko.observable(true);
    this.showWishedBooksButton = ko.observable(true);

    this.hideOwnedBooks = function () {
        self.showOwnedBooks(false);
        self.showOwnedBooksButton(true);
    };
    this.hideWishedBooks = function () {
        self.showWishedBooks(false);
        self.showWishedBooksButton(true);
    };

    this.listOfOwnedBooks = ko.observableArray();
    this.listOfWishedBooks = ko.observableArray();

    this.getOwnedBooks = function () {
        self.listOfOwnedBooks.removeAll();
        send("/profile/get-user-books", { userId: self.user.Id }, function (response) {
            $.each(response, function (index, book) {
                self.listOfOwnedBooks.push(book);
            });
        });
        self.showOwnedBooks(true);
        self.showOwnedBooksButton(false);
    };
    this.getWishedBooks = function () {
        self.listOfWishedBooks.removeAll();
        send("/profile/get-user-wish-books", { userId: self.user.Id }, function (response) {
            $.each(response, function (index, book) {
                self.listOfWishedBooks.push(book);
            });
        });
        self.showWishedBooks(true);
        self.showWishedBooksButton(false);
    };

    this.sendMessage = function () {
        window.location = "/profile/message/to/" + self.user.Id;
    };

    this.getOwnedBooks();
    this.getWishedBooks();
};