define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    var activeComponent = ko.observable("search-results");
    var results = ko.observable();

    ds.search((data) => {
        results(data)
    });

    var selectPost = (data, searchResult) => {
        console.log(searchResult);
        store.dispatch(store.actions.selectPost(searchResult.id, searchResult.isQuestion));
    }

    return function () {
        return {
            activeComponent,
            selectPost,
            results
        };
    };
});
