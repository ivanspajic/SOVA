define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    var activeComponent = ko.observable("search-results");
    var results = ko.observableArray([]);

    ds.getResults((data) => {
        results(data.items.$values);
    });

    var search = function () {
        var queryTerm = document.getElementById("searchterm").value;
        ds.search(queryTerm, (data) => {
            results(data);
        }
    };

    var selectResult = (data, searchResult) => {
        store.dispatch(store.actions.selectResult(searchResult.Id));
    }

    return function () {
        return {
            activeComponent,
            results,
            selectResult,
            queryTerm
        };
    };
});
