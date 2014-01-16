var SettingsViewModel = function (data) {
    var self = this;

    self.model = ko.mapping.fromJS(data);
    
    self.applyButtonIsVisible = ko.observable(false);

    self.ItemChanged = function () {
        if (!self.applyButtonIsVisible()) {
            self.applyButtonIsVisible(true);
        }
    };
    

    self.applySettings = function () {
        sendJson("/Profile/Settings/Update/Notifications", self.model, function () {
            self.applyButtonIsVisible(false);
        });
    };
};
