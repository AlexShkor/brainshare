function RegisterModel(data, submitUrl) {
    var self = this;
    this.model = ko.mapping.fromJS(data);

    this.updateResults = function () {
        self.model.locality($("#locality").val());
        self.model.country($("#country").val());
        self.model.formatted_address($("#formatted_address").val());
    };

    this.save = function () {
        self.updateResults();
        sendModel(submitUrl, self.model, function (model) {

            if (model.Errors.length == 0) {
                window.location.href = "/User/Login";
            }
        });
    };

}