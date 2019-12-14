define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    return function () {
        var errorMessage = ko.observable();
        var username = ko.observable();
        var password = ko.observable();

        var createUser = function () {
            ds.createUser(username(), password(), (data) => {
                if (!username() || !password()) {
                    errorMessage(`Please provide all fields.`);
                }
                else if (data.message && data.message.toLowerCase().includes("taken")) {
                    errorMessage(`Someone already has that username. Try another? <br \> Suggestions: <strong>${username()}69 </strong> or <strong>${username()}420</strong>`);
                } else {
                    store.dispatch(store.actions.login(`Welcome ${username()}. Please log in continue.`));
                }
            });
        }
        return {
            createUser,
            errorMessage,
            username,
            password
        }
    }
});