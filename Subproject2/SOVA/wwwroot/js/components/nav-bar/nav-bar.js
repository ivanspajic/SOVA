define(['knockout', 'store', 'dataService'], function (ko, store, ds) {
    return function () {
        var activeComponent = ko.observable("login-page");
        var menuElements = [
            {
                name: "Login",
                component: "login-page"
            }
        ];
        var currentMenu = ko.observable(menuElements[0]);
        var currentUser = ko.observable();
        var authenticationToken = ko.observable();

        var results = ko.observableArray([]);

        var search = function () {
            var queryTerm = document.getElementById("searchterm").value;
            ds.search(queryTerm, (data) => {
                results(data);
            });
        };

        store.subscribe(() => {
            authenticationToken(store.getState().token);
            currentUser(store.getState().username);
            var menuName = store.getState().selectedMenu;
            var menu = menuElements.find(x => x.name === menuName);
            if (menu) {
                currentMenu(menu);
                activeComponent(menu.component);
            }
            activeComponent(store.getState().activeComponent);
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
            authenticationToken,
            search
        };
    };
});