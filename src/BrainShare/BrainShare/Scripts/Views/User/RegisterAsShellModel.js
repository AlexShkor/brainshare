function RegisterAsShellModel(data, submitUrl) {
    var self = this;
    this.model = ko.mapping.fromJS(data);

    this.save = function () {
        console.log(ko.mapping.toJS(self.model));
        sendModel(submitUrl, self.model, function (model) {

            if (model.Errors.length == 0) {
                window.location.href = "/User/Login";
            }
        });
    };

    this.updateResults = function () {
        self.model.Lng($("#lng").val());
        self.model.Lat($("#lat").val());
        self.model.Route($("#route").val());
        self.model.Country($("#country").val());
        self.model.StreetNumber($("#street_number").val());
        self.model.FormattedAddress($("#formatted_address").val());
    };
}