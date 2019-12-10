define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    return function () {

        var activeComponent = ko.observable("signup-page");
        var errorMessage = ko.observable();

        var createUser = function () {
            var username = document.getElementById("username").value;
            var password = document.getElementById("password").value;
            ds.createUser(username, password, (data) => {
                console.log(data);
                if (data.message && data.message.toLowerCase().includes("taken")) {
                    errorMessage(`Provided username is already taken. Please choose a different one. Suggestions: ${username}69 or ${username}420`);
                } else if (data.message && data.message.toLowerCase().includes("fields")) {
                    errorMessage(`Please provide all fields.`);
                } else {
                    store.dispatch(store.actions.login());
                }
            });
        }
        return {
            activeComponent,
            createUser,
            errorMessage
        }
    }
});