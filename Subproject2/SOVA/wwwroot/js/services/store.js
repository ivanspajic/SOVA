define([], function () {
    const landingPage = "LANDING_PAGE";
    const selectQuestion = "SELECT_QUESTION";
    const selectMenu = "SELECT_MENU";
    const signupUser = "SIGN_UP";
    const authentication = "AUTHENTICATION";
    const searching = "SEARCHING";
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
            case landingPage:
                return Object.assign({}, state, { activeComponent: action.activeComponent, token: getState().token });
            case selectQuestion:
                return Object.assign({}, state, { selectedQuestionId: action.selectedQuestionId, activeComponent: action.activeComponent, token: getState().token });
            case selectMenu:
                return Object.assign({}, state, { selectedMenu: action.selectedMenu, activeComponent: action.activeComponent, token: getState().token });
            case signupUser:
                return Object.assign({}, state, { activeComponent: action.activeComponent, token: getState().token });
            case authentication:
                return Object.assign({}, state, { token: action.token, username: action.username, activeComponent: action.activeComponent });
            case searching:
                return Object.assign({}, state, { activeComponent: action.activeComponent, token: getState().token, searchTerm: action.searchTerm});
            default:
                return state;
        }
    };

    var dispatch = function (action) {
        currentState = reducer(currentState, action);
        subscribers.forEach(callback => callback());
    };

    var actions = {
        landingPage: function (token) {
            return {
                type: landingPage,
                activeComponent: "landing-page",
            }
        },
        selectQuestion: function (questionId) {
            return {
                type: selectQuestion,
                selectedQuestionId: questionId,
                activeComponent: "question-with-answers",
            };
        },
        selectMenu: function (menu) {
            return {
                type: selectMenu,
                selectedMenu: menu,
                activeComponent: "login-page"
            };
        },
        searching: function (queryTerm) {
            return {
                type: searching,
                searchTerm: queryTerm,
                activeComponent: "search-results"
            };
        },
        signupUser: function () {
            return {
                type: signupUser,
                activeComponent: "signup-page"
            }
        },
        authentication: function (token, username) {
            return {
                type: authentication,
                token: token,
                username: username,
                activeComponent: "landing-page"
            }
        }
    };

    return {
        getState,
        subscribe,
        dispatch,
        actions
    };
});