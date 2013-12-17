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

    self.setTabState = function(title,state) {
        for (var i = 0; i < self.model.Tabs().length; i++) {
            if (self.model.Tabs()[i].Title() == title) {
                self.model.Tabs()[i].IsActive(state);
            }
        }
    };

    self.switchTab = function (tab) {
        document.location = "#/" + tab.Name();
    };
    
    self.loadTab = function (name,title) {
        getHtml("/Profile/Settings/Tabs/" + name, {}, function (content) {
            self.clearTabSelection();
            $($(".settings-body-wrapper")[0]).html(content);
            self.setTabState(title, true);
        });
    };
    
    Sammy(function () {
        
        this.get('#/common', function () {
            self.loadTab("common", "Общие");
        });
        
        this.get('#/notifications', function () {
            self.loadTab("notifications", "Оповещения");
        });
        
        this.get('#/privacy', function () {
            self.loadTab("privacy", "Приватность");
        });

        if (location.hash == "") {
            document.location = "#/common";
        }

    }).run();
};