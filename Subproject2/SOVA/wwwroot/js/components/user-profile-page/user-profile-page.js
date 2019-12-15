define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    return function () {
        var currentUser = ko.observable(localStorage.getItem("username"));
        store.subscribe(() => {
            if (!currentUser()) {
                currentUser(store.getState().username);
            }
        });

        var userSearchHistory = function () {
            var userSearchHistoryAction = store.actions.userSearchHistory(currentUser());
            store.dispatch(userSearchHistoryAction);
        }
        var userAnnotations = function () {
            store.dispatch(store.actions.userAnnotations(currentUser()));
        }
        var userBookmarks = function () {
            store.dispatch(store.actions.userBookmarks(currentUser()));
        }
        return {
            userSearchHistory,
            userAnnotations,
            userBookmarks,
            currentUser
        }
    }
});