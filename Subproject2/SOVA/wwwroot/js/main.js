require.config({
    baseUrl: "js",
    paths: {
        knockout: "../lib/knockout/build/output/knockout-latest.js",
    }
});

require(["knockout", "app"], function (ko, app) {
    console.log(app.name);
    ko.applyBindings(app);
});