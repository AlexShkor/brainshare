function EditProfileModel(data, submitUrl) {
    var self = this;

    this.model = ko.mapping.fromJS(data);
    this.save = function () {

        var formatted_address = $("#formatted_address").val();
        var country = $("#country").val();
        var locality = $("#locality").val();

        self.model.formatted_address(formatted_address);
        self.model.country(country);
        self.model.locality(locality);


        sendModel(submitUrl, self.model, function (model) {
            if (model.Errors.length == 0) {
                window.location.href = "/Profile/Index";
            }
        });
    };

    this.ShowLocationInput = ko.observable(false);

    this.AddressInputChanged = function () {


    };

    this.EditLocation = function () {
        self.ShowLocationInput(true);

        self.model.original_address(null);
        self.model.formatted_address(null);
        self.model.country(null);
        self.model.locality(null);
    };
}
