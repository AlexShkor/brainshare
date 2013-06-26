$(function () {
    $("#selectorToggle").click(function () {
        $.ajax({
            url: "/User/GetFbFriends",
            success: function (response) {
                $("#fbFriendsSelector").append(response);
                $("#fbFriendsSelector").draggable();
                $("#TDFriendSelector_buttonOK").click(function () {
                    $("#TDFriendSelector").remove();
                });
            }
        });
    });
});
