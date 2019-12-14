define(["knockout", "store"], function (ko, store) {
    return function () {
        var activeComponent = ko.observable("user-search-history");
        var username = ko.observable();
        var token = ko.observable();
        store.subscribe(function () {
            username(store.getState().username);
        });
        return {
            activeComponent,
            username,
            token
        };
    };
});