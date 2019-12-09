define([], function () {
    const selectQuestion = "SELECT_QUESTION";
    const selectMenu = "SELECT_MENU";
    const signupUser = "SIGN_UP";
    const landingPage = "LANDING_PAGE";
    var subscribers = [];
    var currentState = {};
    var getState = () => currentState;

    var subscribe = function (callback) {
        subscribers.push(callback);

        return function () {
            subscribers = subscribers.filter(x => x !== callback);
        };
    };

    var reducer = function (state, action) {
        switch (action.type) {
            case selectQuestion:
                return Object.assign({}, state, { selectedQuestionId: action.selectedQuestionId, activeComponent: "question-with-answers" });
            case selectMenu:
                return Object.assign({}, state, { selectedMenu: action.selectedMenu, activeComponent: "login-page" });
            case signupUser:
                return Object.assign({}, state, { activeComponent: "signup-page" });
            case landingPage:
                return Object.assign({}, state, { activeComponent: "landing-page" });
            default:
                return state;
        }
    };

    var dispatch = function (action) {
        currentState = reducer(currentState, action);
        subscribers.forEach(callback => callback());
    };

    var actions = {
        selectQuestion: function (questionId) {
            return {
                type: selectQuestion,
                selectedQuestionId: questionId,
                activeComponent: "question-with-answers"
            };
        },
        selectMenu: function (menu) {
            return {
                type: selectMenu,
                selectedMenu: menu,
                activeComponent: "login-page"
            };
        },
        signupUser: function (menu) {
            return {
                type: signupUser,
                activeComponent: "signup-page"
            }
        },
        landingPage: {
            type: landingPage,
            activeComponent: "landing-page"
        }
    };

    return {
        getState,
        subscribe,
        dispatch,
        actions
    };
});