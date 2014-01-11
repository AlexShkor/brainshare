var LinkAccountViewModel = function (data, submitUrl) {
    var self = this;
    this.model = ko.mapping.fromJS(data);
    this.save = function () {
        sendModel(submitUrl, self.model, function (model) {
            if (model.Errors.length == 0) {
                window.location.href = "/Profile/Index";
            }
        });
    };
};