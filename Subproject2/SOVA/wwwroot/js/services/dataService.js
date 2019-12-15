define(['knockout', 'store'], function (ko, store) {

    var selectedQuestionIdInLocalStorage = localStorage.getItem("selectedQuestionId");
    var searchTermInLocalStorage = localStorage.getItem("searchTerm");
    var selectedTagInLocalStorage = localStorage.getItem("selectedTag");
    var authenticationTokenInLocalStorage = localStorage.getItem("token");

    var selectedQuestionId = !!selectedQuestionIdInLocalStorage ? ko.observable(selectedQuestionIdInLocalStorage) : ko.observable(store.getState().selectedQuestionId);
    var selectedPostId = ko.observable(store.getState().selectedPostId);
    var selectedTag = !!selectedTagInLocalStorage ? ko.observable(selectedTagInLocalStorage) : ko.observable(store.getState().selectedTag);
    var authenticationToken = !!authenticationTokenInLocalStorage ? ko.observable(`Bearer ${localStorage.getItem('token')}`) : ko.observable();
    var searchTerm = !!searchTermInLocalStorage ? ko.observable(searchTermInLocalStorage) : ko.observable();

    store.subscribe(function () {
        authenticationToken(`Bearer ${localStorage.getItem('token')}`);
        selectedQuestionId(store.getState().selectedQuestionId);
        selectedPostId(store.getState().selectedPostId);
        searchTerm(store.getState().searchTerm);
        selectedTag(store.getState().selectedTag);
    });

    var getQuestions = async (callback) => {
        var response = await fetch("api/questions");
        var data = await response.json();
        callback(data);
    }

    var getQuestionsByTag = async (callback) => {
        var response = await fetch(`api/questions/tag/${selectedTag()}`);
        var data = await response.json();
        callback(data);
    }

    var getQuestionByIdWithAnswers = async (callback) => {
        var response;
        if (selectedQuestionId !== undefined)
            response = await fetch(`api/questions/${selectedQuestionId()}`);
        else if (selectedPostId !== undefined)
            response = await fetch(`api/questions/${selectedPostId()}`);
        var data = await response.json();
        callback(data);
    }

    var getAnswerById = async (callback) => {
        var response = await fetch(`api/answers/${selectedPostId()}`);
        var data = await response.json();
        callback(data);
    }

    var getWord2Words = async (callback) => {
        var response = await fetch(`api/questions/wordcloud/${searchTerm()}`);
        var data = await response.json();
        callback(data);
    }

    var authenticateUser = async (username, password, callback) => {
        var response = await fetch("api/auth/tokens", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ username: username, password: password })
        });
        var data = await response.json();
        callback(data);
    }

    var createUser = async (username, password, callback) => {
        var response = await fetch("api/auth/users", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ username: username, password: password })
        });
        var data = await response.json();
        callback(data);
    }

    var search = async (callback) => {
        if (searchTerm()) {
            if (!localStorage.getItem('token')) {
                var response = await fetch(`api/questions/query/no-user/${searchTerm()}`);
                var data = await response.json();
                callback(data);
            }
            else {
                var response = await fetch(`api/questions/query/${searchTerm()}`, {
                    method: "GET",
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": `${authenticationToken()}`
                    }
                });
                var data = await response.json();
                callback(data);
            }
        }
    }

    var getOtherPages = async (link, callback) => {
        if (link()) {
            var newLink = link().replace("https://localhost:5001/", "");

            if (!localStorage.getItem('token')) {
                var response = await fetch(newLink);
                var data = await response.json();
                callback(data);
            }
            else {
                var response = await fetch(newLink, {
                    method: "GET",
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": `${authenticationToken()}`
                    }
                });
                var data = await response.json();
                callback(data);
            }
        }
    }

    var saveAnnotation = async (annotationText, callback) => {
        if (!localStorage.getItem('token')) {
            callback({ message: "Not authorized" });
        }
        else {
            var response = await fetch(`api/annotations/${selectedQuestionId()}`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `${authenticationToken()}`
                },
                body: JSON.stringify({ AnnotationString: annotationText })
            });
            var data = await response.json();
            callback(data);
        }
    }

    var updateAnnotation = async (annotationText, callback) => {
        if (!localStorage.getItem('token')) {
            callback({ message: "Not authorized" });
        }
        else {
            var response = await fetch(`api/annotations/${selectedQuestionId()}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `${authenticationToken()}`
                },
                body: JSON.stringify({ AnnotationString: annotationText })
            });
            var data = await response.json();
            callback(data);
        }
    }

    var deleteAnnotation = async (callback) => {
        if (!localStorage.getItem('token')) {
            callback({ message: "Not authorized" });
        }
        else {
            var response = await fetch(`api/annotations/${selectedQuestionId()}`, {
                method: "DELETE",
                headers: {
                    "Authorization": `${authenticationToken()}`
                }
            });
            var data = "Annotation deleted.";
            callback(data);
        }
    }

    var getAnnotation = async (callback) => {
        if (!localStorage.getItem('token')) {
            callback({ message: "Not authorized" });
        }
        else {
            var response = await fetch(`api/annotations/${selectedQuestionId()}`, {
                headers: {
                    "Authorization": `${authenticationToken()}`
                }
            });
            var data = await response;
            if (data.status === 204) {
                data.message = "not found";
            }
            else {
                data = await response.json();
            }
            callback(data);
        }
    }

    var getAllAnnotationsForUser = async (callback) => {
        if (!localStorage.getItem('token')) {
            callback({ message: "Not authorized" });
        }
        else {
            var response = await fetch(`api/annotations`, {
                headers: {
                    "Authorization": `${authenticationToken()}`
                }
            });
            var data = await response;
            if (data.status === 204) {
                data.message = "not found";
            }
            else {
                data = await response.json();
            }
            callback(data);
        }
    }

    var toggleBookmarkStatus = async (callback) => {
        if (!localStorage.getItem('token')) {
            callback({ message: "Not authorized" });
        } else {
            var response = await fetch(`api/${selectedQuestionId()}/bookmarks`,
                {
                    method: "PUT",
                    headers: {
                        "Authorization": `${authenticationToken()}`
                    }
                });
            var data = await response.json();
            callback(data);
        }
    }

    var checkIfBookmarked = async (callback) => {
        if (!localStorage.getItem('token')) {
            callback({ message: "Not authorized" });
        } else {
            var response = await fetch(`api/${selectedQuestionId()}/checkIfBookmarked`,
                {
                    method: "GET",
                    headers: {
                        "Authorization": `${authenticationToken()}`
                    }
                });
            var data = await response.json();
            callback(data);
        }
    }

    var getUserSearches = async (callback) => {
        var response = await fetch("api/history", {
            method: "GET",
            headers: {
                "Authorization": authenticationToken()
            }
        });
        var data = [];
        if (response.status !== 204) {
            data = await response.json();
        }
        callback(data);
    }

    var getUserBookmarks = async (callback) => {
        var response = await fetch("api/bookmarks", {
            method: "GET",
            headers: {
                "Authorization": authenticationToken()
            }
        });
        var data = [];
        if (response.status !== 204) {
            data = await response.json();
        }
        callback(data);
    }

    return {
        getQuestions,
        getQuestionByIdWithAnswers,
        getQuestionsByTag,
        getOtherPages,
        getAnswerById,
        getWord2Words,
        selectedQuestionId,
        authenticateUser,
        authenticationToken,
        createUser,
        search,
        saveAnnotation,
        updateAnnotation,
        deleteAnnotation,
        getAnnotation,
        toggleBookmarkStatus,
        checkIfBookmarked,
        getUserSearches,
        getUserBookmarks,
        getAllAnnotationsForUser
    };
});
