var ConsiderRequestModel = function (data) {

    var self = this;

    this.yourBook = data.YourBook;
    this.fromUserId = data.FromUserId;
    this.allBooks = ko.observableArray(data.AllBooks);
    this.booksYouNeedTitles = ko.observableArray(data.BooksYouNeedTitles);
    this.fromUser = data.FromUser;

    this.sendMessage = function() {
        window.location = "/profile/message/to/" + self.fromUser.UserId;
    };

    this.reject = function() {
        window.location = "/profile/reject/" + self.yourBook.Id + "/from/" + self.fromUser.UserId;
    };

    this.exchange = function (book) {
        sendJson("/books/exchange", { yourBookId: self.yourBook.Id, bookId: book.Id, userId: self.fromUser.UserId }, function (response) {
            alert(JSON.stringify(response));
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