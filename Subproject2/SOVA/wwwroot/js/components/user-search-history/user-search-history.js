define(["knockout", "store", "dataService"], function (ko, store, ds) {
    return function () {
        var activeComponent = ko.observable(localStorage.getItem("activeComponent"));
        var username = ko.observable(localStorage.getItem("username"));
        var userSearches = ko.observable([]);
        var updateUserSearches = function () {
            ds.getUserSearches(function (data) {
                var temp = [];
                for (var i = 0; i < data.items.$values.length; i++) {
                    temp.push(data.items.$values[i].history.searchTerm);
                }
                userSearches(temp);
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