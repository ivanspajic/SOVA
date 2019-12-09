define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    var activeComponent = ko.observable("search-results");
    var results = ko.observableArray([]);

    ds.getResults((data) => {
        results(data.items.$values);
    });

    var selectResult = (data, searchResult) => {
        store.dispatch(store.actions.selectResult(searchResult.Id));
    }

    return function () {
        return {
            activeComponent,
            results,
            selectResult
        };
    };
});
