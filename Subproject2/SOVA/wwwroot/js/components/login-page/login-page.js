define(['knockout', 'dataService', 'store'], function (ko, ds, store) {

    var activeComponent = ko.observable("login-page");
    var errorMessage = ko.observable();
    var login = function () {
        try {
            var username = document.getElementById("username").value;
            var password = document.getElementById("password").value;

            ds.authenticateUser(username, password, (data) => {
                console.log(data.status);
                if (data.message && data.message.toLowerCase().includes("username")) {
                    errorMessage("Provided username is not registered in the database");
                }
                else if (data.message && data.message.toLowerCase().includes("password")) {
                    errorMessage("Wrong username or password");
                } else {
                    errorMessage(null);
                    store.dispatch(store.actions.authentication(`Bearer ${data.token}`, data.username));
                    store.dispatch(store.actions.landingPage(data.username));
                    localStorage.setItem('token', data.token);
                    localStorage.setItem('username', data.username);
                }
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
            errorMessage
        };
    };
});