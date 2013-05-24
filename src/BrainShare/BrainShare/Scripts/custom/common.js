var send = function(url, model, successCallback) {
    $.ajax({
        url: url,
        type: 'POST',
        traditional: true,
        data: ko.toJS(model),
        //data: {data: model},
        //data: model,
        success: function(response) {
            if (response.Error) {
                alert(response.Error);
            } else {
                successCallback(response);
            }
        }
    });
};

var sendModel = function(url, model, successCallback) {
    send(url, model, function(response) {
        var defaultBehaviour = true;
        if (successCallback) {
            var result = successCallback(response);
            if (result === false) {
                defaultBehaviour = false;
            }
        }
        if (defaultBehaviour) {
            ko.mapping.fromJS(model, { }, response);
        }
    });
};

var parseValues = function (data, callback, prefix) {
    prefix = prefix || "";
    for (var key in data) {
        if (data[key] == null) {
            continue;
        }
        if (typeof data[key] == "object") {
            parseValues(data[key], callback, prefix + key);
        } else {
            callback(prefix + key, ko.utils.unwrapObservable(data[key]));
        }
    }
};

ko.utils.buildQueryUrl = function (data, excludeKeysList, includeDefaultValues) {
    var query = "";
    parseValues(data, function (key, value) {
        if ((value || includeDefaultValues) && !isKeyExcluded(key, excludeKeysList)) {
            query += encodeURIComponent(key) + "=" + encodeURIComponent(value) + "&";
        }
    });
    return query;
};