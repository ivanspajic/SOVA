define(['knockout', 'store'], function (ko, store) {

    var selectedQuestionId = ko.observable(store.getState().selectedQuestionId);


    store.subscribe(function () {
        selectedQuestionId(store.getState().selectedQuestionId);
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

    var getCurrentUser = async (callback) => {
        try {
            var response = await fetch("api/users/currentUser");
            var data = await response.json();
            callback(data);
        } catch (e) {
            return;
        }
    }

    var authenticateUser = async (username, password, callback) => {
        console.log(username);
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

    return {
        getQuestions,
        getQuestionByIdWithAnswers,
        selectedQuestionId,
        getCurrentUser,
        authenticateUser,
    };
});
