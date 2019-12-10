define(["knockout", "store", "dataService"], function (ko, store, ds) {
    return function () {
        var activeComponent = ko.observable("login-page");

        var currentUser = ko.observable(localStorage.getItem("username"));
        var authenticationToken = ko.observable();

        var search = function () {
            var queryTerm = document.getElementById("searchterm").value;
            store.dispatch(store.actions.searching(queryTerm));
        };

        store.subscribe(() => {
            authenticationToken(store.getState().token);
            activeComponent(store.getState().activeComponent);
            if (!currentUser()) {
                currentUser(store.getState().username);
            }
        });

        var login = function () {
            store.dispatch(store.actions.login());
        };

        var logout = function () {
            localStorage.clear();
            store.dispatch(store.actions.authentication(null));
            store.dispatch(store.actions.landingPage(null));
            store.dispatch(store.actions.landingPage());
            location.reload();
        }


        var isSelected = function (menu) {
            return menu === currentMenu() ? "active" : "";
        };

        return {
            activeComponent,
            isSelected,
            currentUser,
            authenticationToken,
            search,
            login,
            logout
        };
    };
});