function EditShellProfileModel(data, submitUrl) {
    console.log(data);
    var self = this;

    this.model = ko.mapping.fromJS(data);

    this.updateResults = function () {
        self.model.Lng($("#lng").val());
        self.model.Lat($("#lat").val());
        self.model.Route($("#route").val());
        self.model.Country($("#country").val());
        self.model.StreetNumber($("#street_number").val());
        self.model.FormattedAddress($("#formatted_address").val());
    };

    this.save = function () {

        self.updateResults();

        sendModel(submitUrl, self.model, function (model) {
            if (model.Errors.length == 0) {
                window.location.href = "/Profile/Index";
            }
        });
    };


    this.ShowLocationInput = ko.observable(false);

    this.addressFieldDescription = ko.computed(function () {
        return self.ShowLocationInput() ? "Укажите новое положение" : "Текущее положение";
    });

    this.AddressInputChanged = function () {


    };

    this.EditLocation = function () {
        self.ShowLocationInput(true);

        self.model.FormattedAddress(null);
        self.model.country(null);
        self.model.locality(null);
    };
}
