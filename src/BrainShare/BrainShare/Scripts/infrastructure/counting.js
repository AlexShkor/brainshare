$(function () {

    var localStorageIsAvailable = isLocalStorageAvailable();

    if (localStorageIsAvailable) {

        var booksCount = localStorage.getItem('booksCount');
        var messagesCount = localStorage.getItem('messagesCount');
        $("#booksCounter").text(booksCount);
        $("#messagesCounter").text(messagesCount);
    }

    setTimeout(function() {
        $.post("/profile/get-new-books-count", function (data) {

            if (data.Result !== undefined) {
                if (localStorageIsAvailable) {
                    localStorage.setItem('booksCount', data.Result);
                }

                $("#booksCounter, #navigationBooksCounter").text(data.Result);
            }
        });

        $.post("/profile/get-new-messages-count", function (data) {
            if (data.Result !== undefined) {
                if (localStorageIsAvailable) {
                    localStorage.setItem('messagesCount', data.Result);
                }

                $("#messagesCounter, #navigationMessagesCounter").text(data.Result);
            }
        });

        $.post("/profile/get-unread-news-count", function (data) {
            if (data.Result !== undefined) {
                if (localStorageIsAvailable) {
                    localStorage.setItem('newsCount', data.Result);
                }

                $("#newsCounter").text(data.Result);
            }
        });
    }, 1);
    
    function isLocalStorageAvailable() {
        try {
            return 'localStorage' in window && window['localStorage'] !== null;
        } catch (e) {
            return false;
        }
    }
});