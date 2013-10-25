var AcceptRequestModel = function (data) {

    var self = this;

    this.yourBook = data.YourBook;
    this.fromUserId = data.FromUserId;
    this.allBooks = ko.observableArray(data.AllBooks);
    debugger 
    this.booksYouNeedTitles = ko.observableArray(data.BooksYouNeedTitles);
    this.fromUser = data.FromUser;

    this.sendMessage = function() {
        window.location = "/profile/message/to/" + self.fromUser.UserId;
    };

    this.reject = function() {
        window.location = "/profile/reject/" + self.yourBook.Id + "/from/" + self.fromUser.UserId;
    };

    this.exchange = function(book) {
        window.location = "/books/give/" + self.yourBook.Id + "/take/" + book.Id + "/from/" + self.fromUser.UserId;
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