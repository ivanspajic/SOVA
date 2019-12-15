define(["knockout", "store", "dataService"], function (ko, store, ds) {
    return function () {
        var activeComponent = ko.observable(localStorage.getItem("activeComponent"));
        var username = ko.observable(localStorage.getItem("username"));
        var userSearches = ko.observable();
        var userSearchesReversed = ko.observable();

        var updateUserSearches = function () {
            ds.getUserSearches(function (data) {
                var temp1 = [];
                var temp2 = [];
                if (data.items) {
                    for (var i = 0; i < data.items.$values.length; i++) {
                        var searchTermToPushToArray = data.items.$values[i].history.searchTerm;
                        if (temp1.indexOf(searchTermToPushToArray) === -1) {
                            temp1.push(data.items.$values[i].history.searchTerm);
                            temp2.push(data.items.$values[i]);
                        }
                    }
                    userSearches(temp2);
                    userSearchesReversed(userSearches().reverse());
                }
            });
        }
        updateUserSearches()
        store.subscribe(function () {
            if (store.getState().activeComponent === "user-search-history") {
                updateUserSearches();
            }
        });

        return {
            activeComponent,
            username,
            userSearches,
            userSearchesReversed
        };
    };
});