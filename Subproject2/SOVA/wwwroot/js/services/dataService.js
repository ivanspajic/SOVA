define(['knockout', 'store'], function (ko, store) {

    var selectedQuestionId = ko.observable(store.getState().selectedQuestionId);
    var selectedPostId = ko.observable(store.getState().selectedPostId);
    var selectedTag = ko.observable(store.getState().selectedTag);
    var authenticationToken = ko.observable();
    var searchTerm = ko.observable();

    store.subscribe(function () {
        authenticationToken(store.getState().token);
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

    var moreQuestions = async (callback) => {
        var response = await fetch("api/questions?page=1&pageSize=10");
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
        var response = await fetch(`api/questions/query/${searchTerm()}`);
        var data = await response.json();
        callback(data);
    }

    var searchOtherPages = async (link, callback) => {
        if (link()) {
            var newLink = link().replace("https://localhost:5001/", "");
            var response = await fetch(newLink);
            var data = await response.json();
            callback(data);
        }
    }

    var saveAnnotation = async (annotationText, questionId, callback) => {
        var response = await fetch(`api/annotations/${questionId}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ AnnotationString: annotationText })
        });
        var data = await response.json();
        callback(data);
    }

    var getAnnotation = async (callback) => {
        var response = await fetch(`api/annotations/${selectedQuestionId()}`);
        var data = await response.json();
        if (data.status === 404) {
            data.message = "not found";
        }
        callback(data);
    }

    return {
        getQuestions,
        getQuestionByIdWithAnswers,
        getQuestionsByTag,
        searchOtherPages,
        getAnswerById,
        getWord2Words,
        selectedQuestionId,
        authenticateUser,
        authenticationToken,
        createUser,
        search,
        moreQuestions,
        saveAnnotation,
        getAnnotation
    };
});
