define([], function () {
    const landingPage = "LANDING_PAGE";
    const selectQuestion = "SELECT_QUESTION";
    const selectMenu = "SELECT_MENU";
    const signupUser = "SIGN_UP";
    const authentication = "AUTHENTICATION";
    const searching = "SEARCHING";
    const selectPost = "SELECT_POST";
    const answerPage = "ANSWER_PAGE";
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
                return Object.assign({}, state, { activeComponent: action.activeComponent, username: action.username });
            case answerPage:
                return Object.assign({}, state, { selectedPostId: action.selectedPostId, activeComponent: action.activeComponent });
            case selectQuestion:
                return Object.assign({}, state, { selectedQuestionId: action.selectedQuestionId, activeComponent: action.activeComponent });
            case selectMenu:
                return Object.assign({}, state, { selectedMenu: action.selectedMenu, activeComponent: action.activeComponent });
            case selectPost:
                return Object.assign({}, state, { selectedQuestionId: action.selectedQuestionId, selectedPostId: action.selectedPostId, isQuestion: action.isQuestion, activeComponent: action.activeComponent });
            case signupUser:
                return Object.assign({}, state, { activeComponent: action.activeComponent });
            case landingPage:
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
        landingPage: function (username) {
            return {
                type: landingPage,
                activeComponent: "landing-page",
                token: username
            }
        },
        answerPage: function (postId) {
            return {
                type: answerPage,
                activeComponent: "individual-answer",
                selectedPostId: postId                
            }
        },
        selectQuestion: function (questionId) {
            return {
                type: selectQuestion,
                selectedQuestionId: questionId,
                activeComponent: "question-with-answers"
            };
        },
        selectPost: function (postId, isQ) {
            switch (isQ) {
                case true:
                    return {
                        type: selectPost,
                        selectedPostId: postId,
                        isQuestion: isQ,
                        activeComponent: "question-with-answers",
                        selectedQuestionId: postId
                    };
                default:
                    return {
                        type: selectPost,
                        selectedPostId: postId,
                        isQuestion: isQ,
                        activeComponent: "individual-answer",
                        selectedQuestionId: undefined
                    };
            }
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