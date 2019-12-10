define(["knockout", "store", "dataService"], function (ko, store, ds) {
    return function () {
        var activeComponent = ko.observable("login-page");
        var menuElements = [
            {
                name: "Login",
                component: "login-page"
            }
        ];
        var currentMenu = ko.observable(menuElements[0]);
        var currentUser = ko.observable(localStorage.getItem("username"));
        var authenticationToken = ko.observable();

        store.subscribe(() => {
            authenticationToken(store.getState().token);
            var menuName = store.getState().selectedMenu;
            var menu = menuElements.find(x => x.name === menuName);
            if (menu) {
                currentMenu(menu);
                activeComponent(menu.component);
            }
            activeComponent(store.getState().activeComponent);
            if (!currentUser()) {
                currentUser(store.getState().username);
            }
        });

        var changeContent = function (menu) {
            store.dispatch(store.actions.selectMenu(menu.name));
        };


        var isSelected = function (menu) {
            return menu === currentMenu() ? "active" : "";
        };

        return {
            activeComponent,
            changeContent,
            menuElements,
            isSelected,
            currentMenu,
            currentUser,
            authenticationToken
        };
    };
});