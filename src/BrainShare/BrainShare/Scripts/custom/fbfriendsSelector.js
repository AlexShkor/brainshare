
function FacebookSelectorModel() {
    var self = this;

    this.ShowFriendsLoading = ko.observable(false);
    

    $(function () {
        $("#fbFriendsSelectorLink").click(function () {
            self.ShowFriendsLoading(true);
            $.ajax({
                url: "/User/GetFbFriends",
                success: function (response) {

                    $("#fbFriendsSelector").append(response);
                    self.ShowFriendsLoading(false);
                    $("#fbFriendsSelectorLink").unbind("click");

                    $("#fbFriendsSelectorLink").on("click", function () {
                        $("#TDFriendSelector").show();
                    });

                    $("#TDFriendSelector_buttonOK").on("click", function () {
                        $("#TDFriendSelector").hide();
                    });
                }
            });
        });
    });

}

