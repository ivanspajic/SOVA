define(["knockout"], function (ko) {
    const landingPage = "LANDING_PAGE";
    const selectQuestion = "SELECT_QUESTION";
    const login = "LOGIN";
    const tagFilter = "TAG_FILTER";
    const signupUser = "SIGN_UP";
    const authentication = "AUTHENTICATION";
    const searching = "SEARCHING";
    const userSearchHistory = "USER_SEARCH_HISTORY";
    const selectPost = "SELECT_POST";
    const answerPage = "ANSWER_PAGE";
    const userProfilePage = "USER_PROFILE_PAGE";
    const userAnnotations = "USER_ANNOTATIONS";
    const userBookmarks = "USER_BOOKMARKS";
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
        localStorage.setItem("activeComponent", action.activeComponent)
        switch (action.type) {
            case landingPage:
                return Object.assign({}, state, { activeComponent: action.activeComponent, username: action.username });
            case tagFilter:
                localStorage.setItem("selectedTag", action.selectedTag);
                return Object.assign({}, state, { activeComponent: action.activeComponent, selectedTag: action.selectedTag });
            case answerPage:
                localStorage.setItem("selectedPostId", action.selectedPostId);
                return Object.assign({}, state, { selectedPostId: action.selectedPostId, activeComponent: action.activeComponent });
            case selectQuestion:
                localStorage.setItem("selectedQuestionId", action.selectedQuestionId);
                return Object.assign({}, state, { selectedQuestionId: action.selectedQuestionId, activeComponent: action.activeComponent });
            case login:
                return Object.assign({}, state, { activeComponent: action.activeComponent, optionalMessage: action.optionalMessage });
            case selectPost:
                localStorage.setItem("selectedQuestionId", action.selectedQuestionId);
                localStorage.setItem("selectedPostId", action.selectedPostId);
                localStorage.setItem("isQuestion", action.isQuestion);
                return Object.assign({}, state, { selectedQuestionId: action.selectedQuestionId, selectedPostId: action.selectedPostId, isQuestion: action.isQuestion, activeComponent: action.activeComponent });
            case signupUser:
                return Object.assign({}, state, { activeComponent: action.activeComponent });
            case authentication:
                return Object.assign({}, state, { token: action.token, username: action.username, activeComponent: action.activeComponent });
            case searching:
                return Object.assign({}, state, { activeComponent: action.activeComponent, searchTerm: action.searchTerm });
            case userSearchHistory:
                localStorage.setItem("username", action.username);
                return Object.assign({}, state, { username: action.username, activeComponent: action.activeComponent });
            case userProfilePage:
                localStorage.setItem("username", action.username);
                return Object.assign({}, state, { username: action.username, activeComponent: action.activeComponent });
            case userAnnotations:
                localStorage.setItem("username", action.username);
                return Object.assign({}, state, { username: action.username, activeComponent: action.activeComponent });
            case userBookmarks:
                localStorage.setItem("username", action.username);
                return Object.assign({}, state, { username: action.username, activeComponent: action.activeComponent });
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
                activeComponent: "question-with-answers",
            };
        },
        tagFilter: function (tag) {
            return {
                type: tagFilter,
                selectedTag: tag,
                activeComponent: "tag-filter"
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
        userProfilePage: function (username) {
            return {
                type: userProfilePage,
                username: username,
                activeComponent: "user-profile-page"
            }
        },
        userAnnotations: function (username) {
            return {
                type: userAnnotations,
                username: username,
                activeComponent: "user-annotations"
            }
        },
        userBookmarks: function (username) {
            return {
                type: userBookmarks,
                username: username,
                activeComponent: "user-bookmarks"
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