define(['knockout', 'store'], function (ko, store) {

    var selectedQuestionId = ko.observable(store.getState().selectedQuestionId);

    store.subscribe(function () {
        state = store.getState();
        selectedQuestionId(state.selectedQuestionId);
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

    var bookmark = async (callback) => {
        var response = await fetch(`api/${selectedQuestionId()}/bookmarks`, {method: 'PUT'});
        var data = await response.json();
        callback(data);
    }

    return { getQuestions, getQuestionByIdWithAnswers, bookmark, selectedQuestionId };
});
