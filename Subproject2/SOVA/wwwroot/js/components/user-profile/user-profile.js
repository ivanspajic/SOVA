define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    return function () {
        var currentUser = ko.observable();
        store.subscribe(() => {
            currentUser(store.getState().username);
        });
        return {
            currentUser
        };
    };
});