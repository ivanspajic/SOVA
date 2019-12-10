define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    var activeComponent = ko.observable("search-results");
    var results = ko.observable();

    ds.search((data) => {
        results(data)
    });

    return function () {
        return {
            activeComponent,
            results
        };
    };
});
