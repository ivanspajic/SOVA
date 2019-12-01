﻿require.config({
    baseUrl: "js",
    paths: {
        jquery: "../lib/jquery/dist/jquery",
        knockout: "../lib/knockout/build/output/knockout-latest",
        text: "../lib/requirejs-text/text",
        dataService: "services/dataservice"
    },
    shim: {
        knockout: {
            exports: "ko"
        }
    }
});

require(["knockout"], function (ko) {
    ko.components.register("landing-page", {
        viewModel: { require: "components/landing-page/landing-page" },
        template: { require: "text!components/landing-page/landing-page.html" }
    });
    ko.components.register("question-with-answers", {
        viewModel: {
            require: "components/question-with-answers/question-with-answers"
        },
        template: {
            require:
                "text!components/question-with-answers/question-with-answers.html"
        }
    });
});

require(["knockout", "app"], function (ko, app, ds) {
    ko.applyBindings(app);
});