define(["knockout", "store", "dataService"], function (ko, store, ds) {
    return function () {
        var activeComponentInLocalStorage = localStorage.getItem("activeComponent");

        var activeComponent = !!activeComponentInLocalStorage ? ko.observable(activeComponentInLocalStorage) : ko.observable(store.getState().activeComponent);

        var currentUser = ko.observable(localStorage.getItem("username"));
        var authenticationToken = ko.observable(store.getState().token);
        var queryTerm = ko.observable();

        var search = function () {
            localStorage.setItem("searchTerm", queryTerm());
            store.dispatch(store.actions.searching(queryTerm()));
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

        var goToHome = function () {
            queryTerm(null);
            store.dispatch(store.actions.landingPage());
        }


        return {
            activeComponent,
            currentUser,
            authenticationToken,
            search,
            login,
            logout,
            signUp,
            queryTerm,
            goToHome
        };
    };
});