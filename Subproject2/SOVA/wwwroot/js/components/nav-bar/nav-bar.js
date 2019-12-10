define(["knockout", "store", "dataService"], function (ko, store, ds) {
    return function () {
        var activeComponent = ko.observable(store.getState().activeComponent);

        var currentUser = ko.observable(localStorage.getItem("username"));
        var authenticationToken = ko.observable(store.getState().token);
        var searchTerm = ko.observable();

        var search = function () {
            store.dispatch(store.actions.searching(searchTerm()));
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

        var signUp = function () {
            store.dispatch(store.actions.signupUser());
        }

        var logout = function () {
            localStorage.clear();
            store.dispatch(store.actions.authentication(null));
            store.dispatch(store.actions.landingPage(null));
            store.dispatch(store.actions.landingPage());
            location.reload();
        }


        return {
            activeComponent,
            currentUser,
            authenticationToken,
            search,
            login,
            logout,
            signUp,
            searchTerm
        };
    };
});