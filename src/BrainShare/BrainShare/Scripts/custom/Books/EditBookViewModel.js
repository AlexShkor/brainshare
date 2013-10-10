var EditBookViewModel = function (data, submitUrl) {
    var self = this;
    this.model = ko.mapping.fromJS(data);

    this.save = function () {
        sendModel(submitUrl, self.model, function () {
            // window.location = "/books/info"
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

    this.toMine = function () {
        self.model.IsWhishBook(false);
    };
    this.toWish = function () {
        self.model.IsWhishBook(true);
    };

    this.UploadImg = function () {
        $("#bookImgFile").click();
    };

    $("#bookImgFile").change(function () {
        $(".error-mesage").empty();
        $("#uploadBookImage").click();
    });

    var options = {
        beforeSend: function (formOptions) {
            debugger;

            //self.ShowOverlay(true);

            // hide navigation
            //$("#mainNav").hide();
            //$("#currentUser").hide();

            // clear progress bar
            //$("#bar").width('0%');
            //$("#percent").html("0%");

            // show progress bar
            //$("#loadBar").show();

            // empty container from previous page
            //$("#imgPreviewContainer").empty();

            // cancel loading if 'Cancel' button pressed
            //$("#cancelLoading").on("click", function () {
            //        formOptions.abort();
            //        self.ShowOverlay(false);
            //        $("#loadBar").hide();
            //        $("#mainNav").show();
            //        $('#currentUser').show();
            //    });
        },

        uploadProgress: function (event, position, total, percentComplete) {

            //$("#bar").width(percentComplete + '%');
            //$("#percent").html(percentComplete + '%');
        },

        success: function (response) {
            debugger;


            //$("#bar").width('100%');
            //$("#percent").html('100%');
            //$("#loadBar").hide();

            if (response.error) {
                //$(".error-mesage").text(response.error);
                //self.ShowOverlay(false);
                //$("#loadBar").hide();
                //$("#mainNav").show();
                //$('#currentUser').show();
                //self.ShowChangeAvatarButton(true);
                //return;
            }

            if (!response) {
                //self.ShowOverlay(false);
                //$("#loadBar").hide();
                //$("#mainNav").show();
                //$('#currentUser').show();
                //self.ShowChangeAvatarButton(true);
                //return;
            }

            //self.ShowChangeAvatarButton(false);

            //$("#imgPreviewContainer").append($("<img />").attr("id", "avatarPreview").attr("src", response.avatarUrl));

            var t = $("#bookImgContainer");
            //$("#bookImgContainer").attr("src", response.bookImgUrl);
            self.model.Image(response.bookImgUrl);
            //$("#bookImgContainer").append($('<img style="width: 130px; height: 180px;" />').attr("id", "avatarPreview").attr("src", response.bookImgUrl));

            //imgAreaApi = $('#avatarPreview').imgAreaSelect({
            //    aspectRatio: '1:1',
            //    handles: true,
            //    instance: true,
            //    x1: 15,
            //    y1: 15,
            //    x2: 215,
            //    y2: 215,
            //    minWidth: 200,
            //    minHeight: 200,
            //    onSelectChange: preview,
            //});

            $("#bookImgId").val(response.bookImgId);
            $("#bookImgFormat").val(response.bookImgFormat);

            //self.ShowPreviewControls(true);
        },

        complete: function (response) {

        },

        error: function (response) {

        }
    };

    $("#bookImgForm").ajaxForm(options);

};