define(["store"], function (store) {
    const questionWithAnswersComponent = "question-with-answers";
    const loginPageComponent = "login-page";
    const signupPageComponent = "signup-page";
    const searchPageComponent = "search-results";
    var popState = false;
    store.subscribe(function () {
        var state = store.getState();
        var stringifiedState = JSON.stringify(state);
        if (!popState) {
            window.history.pushState(stringifiedState, null);
        } else {
            popState = false;
        }
    });
    window.onpopstate = function (event) {
        console.log(event.state);
        popState = true;
        if (event.state === null) {
            var firstLandingPageAction = store.actions.landingPage();
            store.dispatch(firstLandingPageAction);
        } else {
            var parsedState = JSON.parse(event.state);
            var action = getMatchingAction(parsedState);
            store.dispatch(action);
        }
    }
    var getMatchingAction = function (state) {
        switch (state.activeComponent) {
            case questionWithAnswersComponent:
                return store.actions.selectQuestion(state.selectedQuestionId);
            case loginPageComponent:
                return store.actions.selectMenu(state.selectedMenu);
            case signupPageComponent:
                return store.actions.signupUser();
            case searchPageComponent:
                return store.actions.searching(state.searchTerm);
            default:
                return store.actions.landingPage();
        }
    }
});