define(["knockout"], function (ko) {
    var activeComponent = ko.observable("landing-page");
    var activeParams = ko.observable({});

    return { activeComponent, activeParams };
});