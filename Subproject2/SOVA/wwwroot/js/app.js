define(["knockout", "store"], function (ko, store) {
    var activeComponent = ko.observable("landing-page");
    var activeParams = ko.observable({});

    var menuElements = [
        {
            name: "Login",
            component: "login-page"
        }];

    var currentMenu = menuElements[0];

    var changeContent = function (menu, questionId) {
        store.dispatch(store.actions.selectQuestion(questionId));
        store.dispatch(store.actions.selectMenu(menu.name));
    };

    store.subscribe(() => {
        activeComponent("question-with-answers");
        var menuName = store.getState().selectedMenu;
        var menu = menuElements.find(x => x.name === menuName);
        if (menu) {
            currentMenu(menu);
            currentComponent(menu.component);
        }
    });

    var isSelected = function (menu) {
        return menu === currentMenu() ? "active" : "";
    };

    return { activeComponent, activeParams, changeContent, menuElements, isSelected };
});