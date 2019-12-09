define(['knockout', 'dataService', 'store'], function (ko, ds, store) {

    var activeComponent = ko.observable("login-page");

    var signupUser = function () {
        ds.authenticateUser();        //store.dispatch(store.actions.signupUser());
    };

    return function () {
        return {
            activeComponent,
            signupUser
        };
    };
});