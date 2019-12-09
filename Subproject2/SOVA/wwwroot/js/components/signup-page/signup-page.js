define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    return function () {

        var activeComponent = ko.observable("signup-page");

        var createUser = function () {
            var username = document.getElementById("username").value;
            var password = document.getElementById("password").value;
            ds.createUser(username, password);
            store.dispatch(store.actions.selectMenu());
        }
        return {
            activeComponent,
            createUser
        }
    }
});