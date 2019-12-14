define(["knockout", "store", "dataService"], function (ko, store, ds) {
    return function () {
        var activeComponent = ko.observable(localStorage.getItem("activeComponent"));
        var username = ko.observable(localStorage.getItem("username"));
        var userSearches = ko.observable([]);
        var updateUserSearches = function () {
            ds.getUserSearches(function (data) {
                userSearches(data);
            });
        }
        updateUserSearches();
        store.subscribe(function () {
            if (store.getState().activeComponent == "user-search-history") {
                updateUserSearches();
            }
        });
        return {
            activeComponent,
            username,
            userSearches
        };
    };
});