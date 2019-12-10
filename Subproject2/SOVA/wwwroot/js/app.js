define(["knockout", "store"], function (ko, store) {
    var activeComponent = ko.observable("landing-page");
    var activeParams = ko.observable({});

    var changeContent = function (menu, term, questionId) {
        store.dispatch(store.actions.selectQuestion(questionId));
        store.dispatch(store.actions.selectMenu(menu));
        store.dispatch(store.actions.selectResult(term));
    };

    store.subscribe(() => {
        activeComponent(store.getState().activeComponent);
    });

    return { activeComponent, activeParams, changeContent};
});