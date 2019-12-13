﻿define([], function () {
    const landingPage = "LANDING_PAGE";
    const selectQuestion = "SELECT_QUESTION";
    const login = "LOGIN";
    const signupUser = "SIGN_UP";
    const authentication = "AUTHENTICATION";
    const searching = "SEARCHING";
    const userSearchHistory = "USER_SEARCH_HISTORY";
    const selectPost = "SELECT_POST";
    const answerPage = "ANSWER_PAGE";
    const messageToShow = "MESSAGE_TO_SHOW";
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
                return Object.assign({}, state, { selectedQuestionId: action.selectedQuestionId, activeComponent: action.activeComponent, errorMessage: action.badMessage, successMessage: action.goodMessage });
            case login:
                return Object.assign({}, state, { activeComponent: action.activeComponent, optionalMessage: action.optionalMessage });
            case selectPost:
                return Object.assign({}, state, { selectedQuestionId: action.selectedQuestionId, selectedPostId: action.selectedPostId, isQuestion: action.isQuestion, activeComponent: action.activeComponent });
            case signupUser:
                return Object.assign({}, state, { activeComponent: action.activeComponent });
            case authentication:
                return Object.assign({}, state, { token: action.token, username: action.username, activeComponent: action.activeComponent });
            case searching:
                return Object.assign({}, state, { activeComponent: action.activeComponent, searchTerm: action.searchTerm });
            case userSearchHistory:
                return Object.assign({}, state, { activeComponent: action.activeComponent });
            case messageToShow:
                return Object.assign({}, state, { errorMessage: action.badMessage, successMessage: action.goodMessage });
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
                username: username
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
        login: function (message) {
            return {
                type: login,
                activeComponent: "login-page",
                optionalMessage: message
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
        },
        userSearchHistory: function (username) {
            return {
                type: userSearchHistory,
                username: username,
                activeComponent: "user-search-history"
            }
        },
        messageToShow: function (badMessage, goodMessage) {
            console.log(`good: ${goodMessage}`);
            console.log(`bad: ${badMessage}`)
            return {
                type: messageToShow,
                errorMessage: badMessage,
                successMessage: goodMessage
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