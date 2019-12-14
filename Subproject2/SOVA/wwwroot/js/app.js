define(["knockout", "store"], function (ko, store) {
    var activeComponent = !!localStorage.getItem("activeComponent") ? ko.observable(localStorage.getItem("activeComponent")) : ko.observable("landing-page");
    var activeParams = ko.observable({});

    var changeContent = function (questionId) {
        store.dispatch(store.actions.selectQuestion(questionId));
    };

    store.subscribe(() => {
        activeComponent(store.getState().activeComponent);
    });

    goToLandingPage = () => {
        localStorage.setItem("activeComponent", "landing-page");
        store.dispatch(store.actions.landingPage());
    }


    return {
        activeComponent,
        activeParams,
        changeContent,
        goToLandingPage
    };
});