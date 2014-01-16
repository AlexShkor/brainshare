function ChangePasswordModel(data, submitUrl) {
    var self = this;

    this.model = ko.mapping.fromJS(data);
    this.changePassword = function () {

        sendModel(submitUrl, self.model, function (model) {
            console.log(model);
            if (model.Errors.length == 0) {
                window.location.href = "/User/Login";
            }
        });
    };

}

