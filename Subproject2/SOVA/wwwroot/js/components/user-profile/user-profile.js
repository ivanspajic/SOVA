define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    return function () {
        var currentUser = localStorage.getItem('username');
        return {
            currentUser
        };
    };
});