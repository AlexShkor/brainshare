var NewsListViewModel = function (data) {
    var self = this;
    this.model = ko.mapping.fromJS(data);

    this.openNews = function (news) {
        if (!news.WasRead()) {
            $.post("/profile/set-news-status-to-read",{ id: news.Id()}, function () {
                news.WasRead(true);
            });           
        }
        news.IsVisible(!news.IsVisible());
    };
}