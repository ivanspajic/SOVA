define([], function () {
    var getQuestions = async (callback) => {
        var response = await fetch("api/questions");
        var data = await response.json();
        callback(data);
    }

    var getQuestionByIdWithAnswers = async (callback, questionId) => {
        var response = await fetch(`api/questions/19`);
        var data = await response.json();
        callback(data, questionId);
    }
    return { getQuestions, getQuestionByIdWithAnswers };
});
