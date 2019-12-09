define(['knockout', 'dataService', 'store'], function (ko, ds, store) {

    var activeComponent = ko.observable("login-page");
    var authenticationToken = ko.observable();


    var login = function () {
        try {
            var username = document.getElementById("username").value;
            var password = document.getElementById("password").value;

            ds.authenticateUser(username, password, (data) => {
                authenticationToken(data.token);
                store.dispatch(store.actions.authentication(data.token));
                store.dispatch(store.actions.landingPage(data.token));
            });
        } catch (e) {

        }
    }

    var signupUser = function () {
        store.dispatch(store.actions.signupUser());
    };

    return function () {
        return {
            activeComponent,
            signupUser,
            login,
            authenticationToken,
        };
    };
});