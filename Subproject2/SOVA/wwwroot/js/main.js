require.config({
    baseUrl: "js",
    paths: {
        jquery: "../lib/jquery/dist/jquery",
        knockout: "../lib/knockout/build/output/knockout-latest",
        text: "../lib/requirejs-text/text",
        dataService: "services/dataservice",
        store: "services/store",
        history: "services/history"
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
    ko.components.register("search-results", {
        viewModel: { require: "components/search-results/search-results" },
        template: { require: "text!components/search-results/search-results.html" }
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
    ko.components.register("user-search-history", {
        viewModel: {
            require: "components/user-page/userSearchHistory"
        },
        template: {
            require: "text!components/user-page/userSearchHistory.html"
        }
    })
});

require(["knockout", "app", "store", "history"], function (ko, app, store, history) {
    store.subscribe(() => console.log(store.getState()));
    ko.applyBindings(app);
});
