﻿require.config({
    baseUrl: "js",
    paths: {
        jquery: "../lib/jquery/dist/jquery",
        knockout: "../lib/knockout/build/output/knockout-latest",
        text: "../lib/requirejs-text/text",
        dataService: "services/dataservice",
        store: "services/store"
    },
    shim: {
        jqcloud: ["jquery"],
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
    ko.components.register("nav-bar", {
        viewModel: { require: "components/nav-bar/nav-bar" },
        template: { require: "text!components/nav-bar/nav-bar.html" }
    });
    ko.components.register("user-profile", {
        viewModel: { require: "components/user-profile/user-profile" },
        template: { require: "text!components/user-profile/user-profile.html" }
    });
    ko.components.register("login-page", {
        viewModel: { require: "components/login-page/login-page" },
        template: { require: "text!components/login-page/login-page.html" }
    });
    ko.components.register("signup-page", {
        viewModel: { require: "components/signup-page/signup-page" },
        template: { require: "text!components/signup-page/signup-page.html" }
    });
});

require(["knockout", "app", "store"], function (ko, app, store) {
    store.subscribe(() => console.log(store.getState()));
    ko.applyBindings(app);
});
