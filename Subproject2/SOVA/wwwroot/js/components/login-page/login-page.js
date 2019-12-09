define(['knockout', 'dataService', 'store'], function (ko, ds, store) {

    var activeComponent = ko.observable("login-page");
    var authenticationToken = ko.observable();


    var login = function () {
        var username = document.getElementById("username").value;
        var password = document.getElementById("password").value;

        store.dispatch(store.actions.authenticateUser(username, password));
        console.log(store.getState().username);
        ds.authenticateUser((data) => {
            authenticationToken(data.token);
        });
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