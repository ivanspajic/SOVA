define(["knockout"], function(ko) {
    var currentComponent = ko.observable("questionWithAnswers");
    var currentParams = ko.observable({});
    var changeContent = () => {
        if (currentComponent() === "landingPage") {
            currentParams({ name: 'Ellen' });
            currentComponent("questionWithAnswers");
        } else {
            currentParams({});
            currentComponent("landingPage");
        }
    };


    return {
        currentComponent,
        currentParams,
        changeContent

    };
})