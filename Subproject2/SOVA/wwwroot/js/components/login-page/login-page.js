define(["knockout", "dataService", "store"], function (ko, ds, store) {

    var activeComponent = ko.observable("login-page");
    var errorMessage = ko.observable();
    var username = ko.observable();
    var password = ko.observable();
    var message = ko.observable(store.getState().optionalMessage);

    store.subscribe(function () {
        message(store.getState().optionalMessage);
    });

    var login = function () {
        ds.authenticateUser(username(), password(), (data) => {
            if (data.message && data.message.toLowerCase().includes("username")) {
                errorMessage("Provided username is not registered in the database");
            }
            else if (data.message && data.message.toLowerCase().includes("password")) {
                errorMessage("Wrong username or password");
            } else {
                store.dispatch(store.actions.authentication(`Bearer ${data.token}`, data.username));
                store.dispatch(store.actions.landingPage(data.username));
                localStorage.setItem("token", data.token);
                localStorage.setItem("username", data.username);
            }
        });
    }

    return function () {
        return {
            activeComponent,
            login,
            errorMessage,
            username,
            password,
            message
        };
    };
});