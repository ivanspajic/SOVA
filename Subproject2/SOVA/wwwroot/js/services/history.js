define(["store"], function (store) {
    const landingPageComponent = "landing-page";
    const questionWithAnswersComponent = "question-with-answers";
    const loginPageComponent = "login-page";
    const signupPageComponent = "signup-page";
    const searchPageComponent = "search-results";
    const answerPageComponent = "individual-answer";
    const userProfileComponent = "user-profile-page";
    const userAnnotationsComponent = "user-annotations";
    const userBookmarksComponent = "user-bookmarks";
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
        popState = true;
        if (event.state === null) {
            var firstLandingPageAction = store.actions.landingPage();
            store.dispatch(firstLandingPageAction);
        } else {
            var parsedState = JSON.parse(event.state);
            var action = getMatchingAction(parsedState);
            if (action) {
                store.dispatch(action);
            }
        }
    }
    var getMatchingAction = function (state) {
        switch (state.activeComponent) {
            case landingPageComponent:
                return store.actions.landingPage();
            case questionWithAnswersComponent:
                return store.actions.selectQuestion(state.selectedQuestionId);
            case loginPageComponent:
                return store.actions.login();
            case signupPageComponent:
                return store.actions.signupUser();
            case searchPageComponent:
                return store.actions.searching(state.searchTerm);
            case answerPageComponent:
                return store.actions.selectPost(state.selectedPostId, state.isQuestion);
            case userProfileComponent:
                return store.actions.userProfilePage(state.username);
            case userAnnotationsComponent:
                return store.actions.userAnnotations(state.username);
            case userBookmarksComponent:
                return store.actions.userBookmarks(state.username);
            default:
                return null;
        }
    }
});