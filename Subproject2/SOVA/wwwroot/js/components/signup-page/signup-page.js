define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    return function () {

        var activeComponent = ko.observable("signup-page");
        var errorMessage = ko.observable();
        var username = ko.observable();
        var password = ko.observable();

        var createUser = function () {
            ds.createUser(username(), password(), (data) => {
                if (data.message && data.message.toLowerCase().includes("taken")) {
                    errorMessage(`Someone already has that username. Try another? <br \> Suggestions: <strong>${username()}69 </strong> or <strong>${username()}420</strong>`);
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
            errorMessage,
            username,
            password
        }
    }
});