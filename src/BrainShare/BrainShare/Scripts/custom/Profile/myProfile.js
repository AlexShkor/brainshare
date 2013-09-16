function MyProfileViewModel() {

    var self = this;
    var imgAreaApi;

    this.UploadImg = function () {
        $("#file").click();
    };

    this.ShowCropButton = ko.observable(false);
    this.ShowChangeAvatarButton = ko.observable(true);
    this.ShowCancelPreviewButton = ko.observable(false);

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

        $("#progress").hide();
        $("#mainNav").show();
        $('#currentUser').show();

        self.ShowPreviewControls(false);
        self.ShowCancelPreviewButton(false);
        self.ShowChangeAvatarButton(true);

        function loadRealAvatar(response) {
            debugger;
            $("#avatarContainer div").empty();
            $("#avatarContainer div").append($("<img />").attr("id", "avatarImg").attr("src", response.url));
        }
    };

    $("#file").change(function () {
        $(".error-mesage").empty();
        $("#uploadImage").click();
    });

    var options = {
        beforeSend: function (formOptions) {
            debugger;

            self.ShowOverlay(true);

            //hide navigation
            $("#mainNav").hide();
            $("#currentUser").hide();

            //clear progress bar
            $("#bar").width('0%');
            $("#percent").html("0%");

            //show progress bar
            $("#loadBar").show();

            // cancel loading if 'Cancel' button pressed
            $("#cancelLoading").on("click", function () {
                formOptions.abort();
            });

            //setTimeout(function() {

            //    $("#cancelLoading").trigger("click");
            //}, 1);

            $("#imgPreviewContainer").empty();
        },

        uploadProgress: function (event, position, total, percentComplete) {

            $("#bar").width(percentComplete + '%');
            $("#percent").html(percentComplete + '%');

        },
        success: function (response) {

            $("#bar").width('100%');
            $("#percent").html('100%');

            $("#loadBar").hide();
            //$("#cancelLoading").hide();

            if (response.error) {
                $(".error-mesage").text(response.error);
                self.ShowLoading(false);
                return;
            }

            self.ShowChangeAvatarButton(false);
            self.ShowCropButton(true);
            self.ShowCancelPreviewButton(true);

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
                onSelectChange: preview,
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



    function preview(img, selection) {
        //var scaleX = 100 / (selection.width || 1);
        //var scaleY = 100 / (selection.height || 1);

        //$('#ferret + div > img').css({
        //    width: Math.round(scaleX * 400) + 'px',
        //    height: Math.round(scaleY * 300) + 'px',
        //    marginLeft: '-' + Math.round(scaleX * selection.x1) + 'px',
        //    marginTop: '-' + Math.round(scaleY * selection.y1) + 'px'
        //});
    }


}