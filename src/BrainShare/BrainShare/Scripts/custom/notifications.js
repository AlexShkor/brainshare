
$(function() {

    if (window.CurrentUserId) {
        var hub = $.connection.notificationsHub;
        hub.client.newMessage = function(e) {
            if (e.UserId != self.userId) {
                self.messages.unshift(e);
            }
        };

        hub.client.requestAccepted = function(e) {
            if (e.Message) {
                $.pnotify({
                    title: e.Title,
                    text: e.Message,
                });
            }
        };

        hub.client.genericText = function(e) {
            $.pnotify({
                title: e.Title,
                text: e.Message,
            });
        };
        $.connection.hub.start(function() {
            hub.server.join(window.CurrentUserId);
        });
    }
});
