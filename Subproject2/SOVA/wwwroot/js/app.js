define(["knockout", "store"], function (ko, store) {
    var activeComponent = ko.observable("landing-page");
    var activeParams = ko.observable({});

    var changeContent = function (questionId) {
        store.dispatch(store.actions.selectQuestion(questionId));
    };

    store.subscribe(() => {
        activeComponent("question-with-answers");
    });

    return { activeComponent, activeParams, changeContent };
});