var SettingsViewModel = function (data) {
    var self = this;

    self.model = ko.mapping.fromJS(data);
    
    self.applyButtonIsVisible = ko.observable(false);

    self.addGroupButtonFieldClick = function () {
        self.model.VkGroupsSettings.NewGroupTemplate.IsVisible(!self.model.VkGroupsSettings.NewGroupTemplate.IsVisible());
    };
    

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
    

    self.AddGroup = function () {
        sendJson("/Profile/Settings/Update/VkGroups", self.model.VkGroupsSettings.NewGroupTemplate, function (result) {
            alert(result);
        });
    };
    self.CancellGroup = function() {
        self.addGroupButtonFieldClick();
    };
};
