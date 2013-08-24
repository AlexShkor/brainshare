function MyProfileViewModel() {

    var self = this;
    var jcropApi;

    this.UploadImg = function () {
        $("#file").click();
    };

    this.ShowCropButton = ko.observable(false);
    this.ShowLoading = ko.observable(false);


    this.ApplyBannerCrop = function () {

        debugger;
        var coords = jcropApi.tellScaled();

        var data = {
            AvatarId: $("#avatarId").val(),
            AvatarFormat: $("#avatarFormat").val(),
            x: coords.x,
            y: coords.y,
            width: coords.w,
            height: coords.h
        };

        $("#avatarPreviewContainer").empty();

        send("/Profile/ResizeAvatar", data, loadRealAvatar);
        self.ShowCropButton(false);

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
        beforeSend: function () {
            $("#avatarPreviewContainer").empty();
            self.ShowCropButton(false);
            self.ShowLoading(true);

        },
        uploadProgress: function (event, position, total, percentComplete) {

        },
        success: function (response) {

            if (response.error) {
                $(".error-mesage").text(response.error);
                self.ShowLoading(false);
                return;
            }

            self.ShowLoading(false);

            $("#avatarPreviewContainer").append($("<img />").attr("id", "avatarPreview").attr("src", response.avatarUrl));
            $("#avatarId").val(response.avatarId);
            $("#avatarFormat").val(response.avatarFormat);

            $("#avatarPreview").Jcrop({
                minSize: [200, 200],
                setSelect: [15, 15, 215, 215],
            }, function () { jcropApi = this; });

            self.ShowCropButton(true);
        },

        complete: function (response) {

        },

        error: function (response) {

        }
    };

    $("#myForm").ajaxForm(options);
}