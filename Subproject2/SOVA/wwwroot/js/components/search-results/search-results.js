define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    var activeComponent = ko.observable("search-results");
    var results = ko.observable();

    ds.search((data) => {
        results(data.items.$values);
    });

    var selectPost = (data, id, isQuestion) => {
        store.dispatch(store.actions.selectPost(id, isQuestion));
    }

    return function () {
        return {
            activeComponent,
            selectPost,
            results
        };
    };
});
