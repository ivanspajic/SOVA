define(['knockout', 'dataService', 'store'], function (ko, ds, store) {

    var activeComponent = ko.observable("login-page");

    var signupUser = function (params) {
        activeComponent = ko.observable("signup-page");
    };

    return function (params) {
        return {
            activeComponent,
            signupUser
        };
    };    
});