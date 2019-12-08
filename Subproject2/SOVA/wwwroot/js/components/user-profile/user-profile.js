define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    return function () {
        var currentUser = ko.observable();

        ds.getCurrentUser((data) => {
            currentUser(data);
        });

        return {
            currentUser
        };
    };
});