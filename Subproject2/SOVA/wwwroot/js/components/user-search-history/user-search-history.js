define("store", function (store) {
    return function () {
        var activeComponent = ko.observable("user-search-history");
        store.subscribe(function () {
            var state = store.getState();
        });
        return {
            activeComponent
        };
    };
});