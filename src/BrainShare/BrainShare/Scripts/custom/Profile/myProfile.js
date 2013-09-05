function MyProfileViewModel() {

    var self = this;
    var jcropApi;

    this.UploadImg = function () {
        $("#file").click();
    };

    this.ShowCropButton = ko.observable(false);
    //this.ShowLoading = ko.observable(false);
    this.ShowChangeAvatarButton = ko.observable(true);
    this.ShowCancelPreviewButton = ko.observable(false);


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
        $("#progress").hide();
        $(".overlay").attr("style", "display: none");
        self.ShowCropButton(false);
        self.ShowCancelPreviewButton(false);
        self.ShowChangeAvatarButton(true);
        //setTimeout($("#avatarImg").show(), 50000);

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

            $("#progress").show();
            $(".overlay").show();
            //clear everything
            $("#bar").width('0%');
            $("#percent").html("0%");


            $("#avatarPreviewContainer").empty();
            //self.ShowCropButton(false);
            //self.ShowLoading(true);

        },
        uploadProgress: function (event, position, total, percentComplete) {

            $("#bar").width(percentComplete + '%');
            $("#percent").html(percentComplete + '%');

        },
        success: function (response) {

            $("#bar").width('100%');
            $("#percent").html('100%');
            //$(".overlay").show();
            //self.ShowLoading(true);

            if (response.error) {
                $(".error-mesage").text(response.error);
                self.ShowLoading(false);
                return;
            }

            //self.ShowLoading(false);
            self.ShowChangeAvatarButton(false);
            self.ShowCropButton(true);
            self.ShowCancelPreviewButton(true);

            $("#avatarPreviewContainer").append($("<img />").attr("id", "avatarPreview").attr("src", response.avatarUrl));

            $("#avatarId").val(response.avatarId);
            $("#avatarFormat").val(response.avatarFormat);

            $("#avatarPreview").Jcrop({
                minSize: [200, 200],
                setSelect: [15, 15, 215, 215],
            }, function () { jcropApi = this; });

        
        },

        complete: function (response) {
            //setTimeout(self.ShowCropButton(true), 10000);
        },

        error: function (response) {

        }
    };

   
    $("#myForm").ajaxForm(options);
}