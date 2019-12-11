define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    var results = ko.observable();

    store.subscribe(() => {
        ds.search((data) => {
            results(data.items.$values);
        });
    });

    ds.search((data) => {
        results(data.items.$values);
    });

    var selectPost = (data, id, isQuestion) => {
        store.dispatch(store.actions.selectPost(id, isQuestion.isQuestion));
    }

    return function () {
        return {
            selectPost,
            results
        };
    };
});
