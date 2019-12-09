define(["knockout", "store"], function (ko, store) {
    var activeComponent = ko.observable("landing-page");
    var activeParams = ko.observable({});

    var changeContent = function (menu, questionId) {
        store.dispatch(store.actions.selectQuestion(questionId));
    };

    store.subscribe(() => {
        activeComponent(store.getState().activeComponent);
    });

    return { activeComponent, activeParams, changeContent};
});