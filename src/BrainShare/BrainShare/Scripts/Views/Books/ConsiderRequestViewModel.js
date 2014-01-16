var ConsiderRequestViewModel = function (data) {

    var self = this;

    this.yourBook = data.YourBook;
    this.fromUserId = data.FromUserId;
    this.allBooks = ko.observableArray(data.AllBooks);
    this.booksYouNeedTitles = ko.observableArray(data.BooksYouNeedTitles);
    this.fromUser = data.FromUser;

    this.exchangeError = ko.observable(false);
    this.exchangeSuccess = ko.observable(false);
    
    this.loading = ko.observable(false);
    this.requestSent = ko.observable(false);

    this.sentBookId = ko.observable(null);


    this.sendMessage = function() {
        window.location = "/profile/message/to/" + self.fromUser.UserId;
    };

    this.reject = function() {
        window.location = "/profile/reject/" + self.yourBook.Id + "/from/" + self.fromUser.UserId;
    };

    var processResponse = function (response) {
        if (response.Success) {
            self.exchangeSuccess(true);
            $("#exchangeSuccess").blur();
        } else {
            self.requestSent(false);
            self.exchangeError(true);
            $("#exchangeError").blur();
        }
        self.loading(false);
    };

    this.exchange = function (book) {
        self.loading(true);
        self.requestSent(true);
        self.sentBookId(book.Id);
        sendJson("/books/exchange", { yourBookId: self.yourBook.Id, bookId: book.Id, userId: self.fromUser.UserId }, function (response) {
            processResponse(response);
        });
    };
    
    this.gift = function () {
        self.loading(true);
        self.requestSent(true);
        sendJson("/books/make-gift", { bookId: self.yourBook.Id, userId: self.fromUser.UserId }, function (response) {
            processResponse(response);
        });
    };
    

    this.viewBook = function (book) {
        window.location = "/books/info/" + book.Id;
    };
    
    this.viewUser = function () {
        window.location = "/Profile/ViewUserProfile/" + data.FromUser.UserId;
    };

    // ViewModel for PartialView
    this.book = self.yourBook;
 }