define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    var activeComponent = ko.observable("individual-answer");


    return function () {
        return {
            activeComponent
        };
    };
});