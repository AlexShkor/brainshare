var SettingsViewModel = function (model) {
    var self = this;

    self.model = ko.mapping.fromJS(model);

    self.clearTabSelection = function () {
        for (var i = 0; i < self.model.Tabs().length; i++) {
            if (self.model.Tabs()[i].IsActive()) {
                self.model.Tabs()[i].IsActive(false);
            }
        }
    };

    self.loadTab = function (tab) {
        console.log(ko.mapping.toJS(tab));
        getHtml("/Profile/Settings/Tabs/" + tab.Name(), {}, function (content) {
            self.clearTabSelection();
            $($(".settings-body-wrapper")[0]).html(content);
            tab.IsActive(true);
        });
    };
};