var send = function (url, data, successCallback) {
    sendJson(url, data, successCallback);
};

var isFunction = function (functionToCheck) {
    var getType = { };
    return functionToCheck && getType.toString.call(functionToCheck) === '[object Function]';
};

var sendModel = function (url, model, successCallback, ignoreList) {
    model.Loading(true);
    var mapping = {
        ignore: ignoreList
    };
    sendJson(url, ko.mapping.toJS(model,mapping), function(response) {
        var defaultBehaviour = true;
        if (successCallback) {
            var result = successCallback(response);
            if (result === false) {
                defaultBehaviour = false;
            }
        }
        if (defaultBehaviour) {
            ko.mapping.fromJS(response, mapping, model);
        }
        model.Loading(false);
    }, function() {
        model.Loading(false);
    });
};

function sendJson(url, jsonOrModel, success, error) {
    var json = ko.mapping.toJS(jsonOrModel);
    $.ajax({
        type: 'POST',
        contentType: 'application/json,UTF-8',
        dataType: "json",
        url: url,
        data: JSON.stringify(json),
        success: function (response) {
            if (success) {
                success(response);
            }
        },
        error: function (response) {
            if (error) {
                error(response);
            }
        }
    });
}

var parseValues = function (data, callback, prefix, postfix) {
    postfix = postfix || "";
    prefix = prefix || "";
    for (var key in data) {
        if (data[key] == null) {
            continue;
        }
        if (Object.prototype.toString.call(data[key]) === '[object Array]') {
            parseValues(data[key], callback, prefix + key + postfix + "[", "]");
        } else if (typeof data[key] == "object") {
            parseValues(data[key], callback, prefix + key + postfix + ".");
        } else {
            callback(prefix + key + postfix, data[key]);
        }
    }
};

ko.utils.buildQueryUrl = function (data, excludeKeysList, includeDefaultValues) {
    var query = "";
    parseValues(data, function (key, value) {
        if ((value || includeDefaultValues) && !isKeyExcluded(key, excludeKeysList)) {
            query += encodeURIComponent(key) + "=" + encodeURIComponent(encodeURIComponent(value)) + "&";
        }
    });
    return query;
};
var isKeyExcluded = function (key, keysList) {
    if (!keysList) {
        return false;
    }
    for (var i = 0; i < keysList.length; i++) {
        if (key.indexOf(keysList[i] + ".") > 0) {
            return true;
        }
    }
    return false;
};

ko.applyContent = function (model) {
    ko.applyBindings(model, $("#content")[0]);
};

ko.applyPopup = function (model) {
    ko.applyBindings(model, $("#modalContent")[0]);
};

// Here's a custom Knockout binding that makes elements shown/hidden via jQuery's fadeIn()/fadeOut() methods
// Could be stored in a separate utility library
ko.bindingHandlers.fadeVisible = {
    init: function (element, valueAccessor) {
        // Initially set the element to be instantly visible/hidden depending on the value
        var value = valueAccessor();
        $(element).toggle(ko.utils.unwrapObservable(value)); // Use "unwrapObservable" so we can handle values that may or may not be observable
    },
    update: function (element, valueAccessor) {
        // Whenever the value subsequently changes, slowly fade the element in or out
        var value = valueAccessor();
        ko.utils.unwrapObservable(value) ? $(element).fadeIn() : $(element).fadeOut();
    }
};

// Here's a custom Knockout binding that makes elements shown/hidden via jQuery's fadeIn()/fadeOut() methods
// Could be stored in a separate utility library
ko.bindingHandlers.slideVisible = {
    init: function (element, valueAccessor) {
        // Initially set the element to be instantly visible/hidden depending on the value
        var value = valueAccessor();
        $(element).toggle(ko.utils.unwrapObservable(value)); // Use "unwrapObservable" so we can handle values that may or may not be observable
    },
    update: function (element, valueAccessor) {
        // Whenever the value subsequently changes, slowly fade the element in or out
        var value = valueAccessor();
        ko.utils.unwrapObservable(value) ? $(element).slideDown("fast") : $(element).slideUp("fast");
    }
};


ko.bindingHandlers.clear = {
    init: function (elem, valueAccessor) {
        $(elem).click(function () {
            var value = valueAccessor();
            if (value.removeAll) {
                value.removeAll();
            } else {
                value(null);
            }

        });
    }
};