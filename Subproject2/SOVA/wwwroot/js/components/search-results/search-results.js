define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    var activeComponent = ko.observable("search-results");
    var results = ko.observableArray([]);

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
