define(["knockout", "store"], function (ko, store) {
    var activeComponent = ko.observable("landing-page");
    var activeParams = ko.observable({});

    var menuElements = [
        {
            name: "Login",
            component: "login-page"
        }];

    var currentMenu = ko.observable(menuElements[0]);

    var changeContent = function (menu, questionId) {
        store.dispatch(store.actions.selectQuestion(questionId));
        store.dispatch(store.actions.selectMenu(menu.name));
    };

    store.subscribe(() => {
        var menuName = store.getState().selectedMenu;
        var menu = menuElements.find(x => x.name === menuName);
        if (menu) {
            currentMenu(menu);
            activeComponent(menu.component);
        }
        activeComponent(store.getState().activeComponent);
    });

    var isSelected = function (menu) {
        return menu === currentMenu() ? "active" : "";
    };

    return { activeComponent, activeParams, changeContent, menuElements, isSelected, currentMenu };
});