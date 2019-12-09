define(["store"], function (store) {
    const questionWithAnswersComponent = "question-with-answers";
    const loginPageComponent = "login-page";
    const signupPageComponent = "signup-page";
    store.subscribe(function () {
        var state = store.getState();
        var stringifiedState = JSON.stringify(state);
        console.log(window.history);
        window.history.pushState(stringifiedState, null);
    });
    window.onpopstate = function (event) {
        console.log(event.state);
        if (event.state === null) {
            var firstLandingPageAction = store.actions.landingPage;
            store.dispatch(firstLandingPageAction);
        } else {
            var parsedState = JSON.parse(event.state);
            var action = getMatchingAction(parsedState.activeComponent);
            if (typeof storeAction === 'function') {
                action = storeAction(parsedState.selectedQuestionId);
            }
            console.log(action);
            store.dispatch(action);
        }
    }
    var getMatchingAction = function (component) {
        switch (component) {
            case questionWithAnswersComponent:
                return store.actions.selectQuestion;
            case loginPageComponent:
                return store.actions.selectMenu;
            case signupPageComponent:
                return store.actions.signupUser;
            default:
                return store.actions.landingPage;
        }
    }
});