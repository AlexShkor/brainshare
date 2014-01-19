var EditBookViewModel = function (data, submitUrl) {
    var self = this;

    this.additionalVisible = ko.observable(false);

    this.model = ko.mapping.fromJS(data);

    this.save = function () {
        sendModel(submitUrl, self.model, function (model) {
            debugger;
            if (model.Errors.length == 0) {
                window.location.href = "/books/info?id=" + model.Id;
            }
        }, ["Languages"]);
    };

    this.addISBN = function () {
        self.model.ISBNs.push({
            Value: ko.observable()
        });
    };
    this.addAuthor = function () {
        self.model.Authors.push({
            Value: ko.observable()
        });
    };
    this.removeAuthor = function (author) {
        if (self.model.Authors().length > 1) {
            self.model.Authors.remove(author);
        }
    };
    this.removeISBN = function (isbn) {
        if (self.model.ISBNs().length > 1) {
            self.model.ISBNs.remove(isbn);
        }
    };

    this.toggleAdditionalFiels = function() {
        self.additionalVisible(!self.additionalVisible());
    };

    this.toMine = function () {
        self.model.IsWhishBook(false);
    };
    this.toWish = function () {
        self.model.IsWhishBook(true);
    };

    this.UploadImg = function () {
        $("#bookImgFile").click();
    };

    this.ImageIsLoading = ko.observable(false);

    $("#bookImgFile").change(function () {
        $(".error-mesage").empty();
        $("#uploadBookImage").click();
    });

    var options = {
        beforeSend: function (formOptions) {
            debugger;
            self.ImageIsLoading(true);
        },

        uploadProgress: function (event, position, total, percentComplete) {
            $("#bookImgBar").width(percentComplete + '%');
        },

        success: function (response) {
            debugger;
            $("#bookImgBar").width('100%');
            self.ImageIsLoading(false);

            if (response.error) {
                $(".error-mesage").text(response.error);
                return;
            }

            if (!response) {
                return;
            }

            self.model.Image(response.bookImgUrl);

        },

        complete: function (response) {

        },

        error: function (response) {

        }
    };

    $("#bookImgForm").ajaxForm(options);
};