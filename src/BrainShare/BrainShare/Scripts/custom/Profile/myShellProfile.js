function MyShellProfileViewModel() {

    var self = this;
    var imgAreaApi;

    this.UploadImg = function () {
        $("#file").click();
    };

    this.ShowChangeAvatarButton = ko.observable(true);

    this.ShowPreviewControls = ko.observable(false);
    this.ShowOverlay = ko.observable(false);

    this.CropImg = function () {
        debugger;
        var selection = imgAreaApi.getSelection();

        var data = {
            AvatarId: $("#avatarId").val(),
            AvatarFormat: $("#avatarFormat").val(),
            x: selection.x1,
            y: selection.y1,
            width: selection.width,
            height: selection.height
        };

        imgAreaApi.remove();
        $("#imgPreviewContainer").empty();


        send("/Profile/ResizeAvatar", data, loadRealAvatar);

        self.ShowOverlay(false);
        $("#mainNav").show();
        $('#currentUser').show();

        self.ShowPreviewControls(false);

        self.ShowChangeAvatarButton(true);

        function loadRealAvatar(response) {
            debugger;
            $("#avatarContainer div").empty();
            $("#avatarContainer div").append($("<img style='width: 250px; height: 250px;' />").attr("id", "avatarImg").attr("src", response.url));
        }
    };

    this.CancelImg = function () {
        imgAreaApi.remove();
        $("#imgPreviewContainer").empty();
        self.ShowPreviewControls(false);

        self.ShowOverlay(false);
        $("#loadBar").hide();
        $("#mainNav").show();
        $('#currentUser').show();
        self.ShowChangeAvatarButton(true);
    };

    $("#file").change(function () {
        $(".error-mesage").empty();
        $("#uploadImage").click();
    });

    var options = {
        beforeSend: function (formOptions) {
            debugger;

            self.ShowOverlay(true);

            // hide navigation
            $("#mainNav").hide();
            $("#currentUser").hide();

            // clear progress bar
            $("#bar").width('0%');
            $("#percent").html("0%");

            // show progress bar
            $("#loadBar").show();

            // empty container from previous page
            $("#imgPreviewContainer").empty();

            // cancel loading if 'Cancel' button pressed
            $("#cancelLoading").on("click", function () {
                formOptions.abort();
                self.ShowOverlay(false);
                $("#loadBar").hide();
                $("#mainNav").show();
                $('#currentUser').show();
            });
        },

        uploadProgress: function (event, position, total, percentComplete) {

            $("#bar").width(percentComplete + '%');
            $("#percent").html(percentComplete + '%');
        },

        success: function (response) {
            debugger;


            $("#bar").width('100%');
            $("#percent").html('100%');
            $("#loadBar").hide();

            if (response.error) {
                $(".error-mesage").text(response.error);
                self.ShowOverlay(false);
                $("#loadBar").hide();
                $("#mainNav").show();
                $('#currentUser').show();
                self.ShowChangeAvatarButton(true);
                return;
            }

            if (!response) {
                self.ShowOverlay(false);
                $("#loadBar").hide();
                $("#mainNav").show();
                $('#currentUser').show();
                self.ShowChangeAvatarButton(true);
                return;
            }

            self.ShowChangeAvatarButton(false);

            $("#imgPreviewContainer").append($("<img />").attr("id", "avatarPreview").attr("src", response.avatarUrl));

            imgAreaApi = $('#avatarPreview').imgAreaSelect({
                aspectRatio: '1:1',
                handles: true,
                instance: true,
                x1: 15,
                y1: 15,
                x2: 215,
                y2: 215,
                minWidth: 200,
                minHeight: 200,
            });

            $("#avatarId").val(response.avatarId);
            $("#avatarFormat").val(response.avatarFormat);

            self.ShowPreviewControls(true);
        },

        complete: function (response) {

        },

        error: function (response) {

        }
    };

    $("#myForm").ajaxForm(options);

}