function ApplyFilterModel(self, urlPart, itemsMapping) {
    self.items = ko.observableArray();
    self.filter.pages = ko.observableArray();

    self.filter.DoNotReturnPropertiesNames = self.filter.DoNotReturnPropertiesNames || ko.observableArray([]);
    self.filter.TotalPages = self.filter.TotalPages || ko.observable(1);
    self.filter.CurrentPage = self.filter.CurrentPage || ko.observable(1);
    self.filter.PagesRange = 4; // number of pages to show before/after current page

    var ignoreList = self.filter.DoNotReturnPropertiesNames() || [];
    ignoreList.push('pages');

    self.isVisiblePage = function (item) {
        var pageNumber = item.Number();
        return pageNumber == 1
            || pageNumber == self.filter.TotalPages()
            || (pageNumber >= self.firstPageInRangeNumber() && pageNumber <= self.lastPageInRangeNumber());
    };

    self.showThreeDots = function (item) {
        var pageNumber = item.Number();
        return pageNumber != 1
            && pageNumber != self.filter.TotalPages()
            && ((pageNumber == self.firstPageInRangeNumber() - 1) || (pageNumber == self.lastPageInRangeNumber() + 1));
    };

    self.firstPageInRangeNumber = ko.computed(function () {
        var selectedPageNumber = self.filter.CurrentPage();
        var result = selectedPageNumber - self.filter.PagesRange;
        if (result <= 1) {
            result = 1;
        } else {
            var additionalPagesCount = selectedPageNumber + self.filter.PagesRange - self.filter.TotalPages();
            if (additionalPagesCount > 0)
                result = result - additionalPagesCount;
        }
        return result;
    });

    self.lastPageInRangeNumber = ko.computed(function () {
        var selectedPageNumber = self.filter.CurrentPage();
        var result = selectedPageNumber + self.filter.PagesRange;
        if (result >= self.filter.TotalPages()) {
            result = self.filter.TotalPages();
        } else {
            var additionalPagesCount = self.filter.PagesRange - selectedPageNumber + 1;
            if (additionalPagesCount > 0)
                result = result + additionalPagesCount;
        }
        return result;
    });

    self.goToPrevPage = function () {
        goToPageNumber(self.filter.CurrentPage() - 1);
    };
    self.goToNextPage = function () {
        goToPageNumber(self.filter.CurrentPage() + 1);
    };

    self.goToPage = function (item) {
        var page = item.Number();
        goToPageNumber(page);
    };

    self.search = function () {
        self.filter.CurrentPage(1);
        submitForm();
    };

    self.updateItems = function () {
        var dto = self.filter.toJS();
        $.get(urlPart + "/filter?" + ko.utils.buildQueryUrl(dto), {}, serverCallback);
    };

    var goToPageNumber = function (page) {
        if (page > 0 && self.filter.CurrentPage() != page && page <= self.filter.TotalPages()) {
            self.filter.CurrentPage(page);
            submitForm();
        }
    };

    self.updatePagination = function () {
        self.filter.pages.removeAll();
        if (self.filter.pages().length != self.filter.TotalPages()) {
            for (var i = 1; i <= self.filter.TotalPages() ; i++) {
                self.filter.pages.push({
                    Number: ko.observable(i)
                });
            }
        }
    };
    self.updatePagination();

    self.sort = function (model, event) {
        var key = $(event.target).data("key") || $(event.target.parentNode).data("key");
        var desc = self.filter.OrderByKey() == key && !self.filter.Desc();
        self.filter.Desc(desc);
        self.filter.OrderByKey(key);
        self.search();
        return true;
    };

    self.filter.toJS = function () {
        return ko.mapping.toJS(self.filter, { 'ignore': ignoreList });
    };

    var submitForm = function () {
        var dto = self.filter.toJS();
        location.hash = "search/" + ko.utils.buildQueryUrl(dto);
    };

    var serverCallback = function (resp) {
        var updateIgnoreList = $.grep(ignoreList, function (value) {
            return value != 'TotalPages';
        });
        ko.mapping.fromJS(resp.Filter, { 'ignore': updateIgnoreList }, self.filter);
        ko.mapping.fromJS(resp.Items, itemsMapping || {}, self.items);
        self.updatePagination();
        self.filter.Loading(false);
    };

    Sammy(function () {


        this.get(/\#search\/(.*)/, function () {
            self.filter.Loading(true);
            $.get(urlPart + "/filter?" + this.params['splat'][0], {}, serverCallback);
        });

        this.get(urlPart, function () {
            self.filter.Loading(true);
            $.get(urlPart + "/filter", {}, serverCallback);
        });

        if (location.hash == "") {
            self.filter.Loading(true);
            $.get(urlPart + "/filter", {}, serverCallback);
        }

    }).run();
}