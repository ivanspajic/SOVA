define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    return function () {
        var currentUser = ko.observable(localStorage.getItem("username"));
        store.subscribe(() => {
            if (!currentUser()) {
                currentUser(store.getState().username);
            }
        });
        return {
            currentUser
        };
    };
});