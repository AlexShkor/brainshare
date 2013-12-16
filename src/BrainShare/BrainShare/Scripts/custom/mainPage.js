
$(function () {
    var isbnRedirect = function () {
        var searchISBNS = encodeURIComponent($("#search").val());
        document.location = "/books/info/" + searchISBNS;
    };

    var searchRedirect = function () {
        var search = encodeURIComponent($($(".selectize-input input")[0]).val());
        document.location = "/Books/Index/" + search;
    };

    $('#search').selectize({
        valueField: 'Id',
        labelField: 'Title',
        searchField: 'Title',
        placeholder: "Искать книги",
        options: [],
        create: false,
        maxItems: 1,
        render: {
            option: function (item, escape) {
                console.log(item);

                return '<div>' +
                    '<img class=autocomplete-image src="' + escape(item.Image) + '" alt="">' +
                    '<span class="name title">' + escape(item.Title) + '</span> </br>' +
                    '<div><span class="owner"> Владелец : ' + item.UserName + '</span></div></br>' +
                    '<div class="name"><span>Расположение : ' + escape(item.UserLocality) + '</span></div>' +
                    '</div>';
            }
        },
        load: function (query, callback) {
            if (!query.length) return callback();
            $.ajax({
                url: '/Books/Filter',
                type: 'GET',
                data: {
                    Search: query,
                },
                error: function () {
                    console.log("error");
                },
                success: function (res) {
                    callback(res.Items);
                }
            });
        }
    }).on('change', isbnRedirect);
});
