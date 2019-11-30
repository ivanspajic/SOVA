require.config({
    baseUrl: "js",
    paths: {
        jquery: "../lib/jquery/dist/jquery",
        knockout: "../lib/knockout/build/output/knockout-latest.debug",
        text: "../lib/requirejs-text/text",
        dataService: "services/dataservice"
    }
});


require(["knockout"], function(ko) {
    ko.components.register("landingPage", {
        viewModel: { require: "components/landingPage/landingPage" },
        template: { require: "text!components/landingPage/landingPage.html" }
    });
    ko.components.register("questionWithAnswers", {
        viewModel: { require: "components/questionWithAnswers/questionWithAnswers" },
        template: { require: "text!components/questionWithAnswers/questionWithAnswers.html" }
    });
});

require(["knockout", "app"], function(ko, app, ds) {
    ko.applyBindings(app);
});