define(['knockout', 'store'], function (ko, store) {

    var selectedQuestionId = ko.observable(store.getState().selectedQuestionId);
    var authenticationToken = ko.observable();
    var queryTerm = document.getElementById("searchterm").value;

    store.subscribe(function () {
        authenticationToken(store.getState().token);
        selectedQuestionId(store.getState().selectedQuestionId);
        queryTerm(store.getState().selectedResult);
    });

    var getQuestions = async (callback) => {
        var response = await fetch("api/questions");
        var data = await response.json();
        callback(data);
    }

    var getQuestionByIdWithAnswers = async (callback) => {
        var response = await fetch(`api/questions/${selectedQuestionId()}`);
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

    var search = async (queryTerm, callback) => {
        var response = await fetch(`/api/questions/query/${queryTerm}`);
        var data = await response.json();
        callback(data);
    }

    return {
        getQuestions,
        getQuestionByIdWithAnswers,
        selectedQuestionId,
        authenticateUser,
        authenticationToken,
        queryTerm,
        search
    };
});
