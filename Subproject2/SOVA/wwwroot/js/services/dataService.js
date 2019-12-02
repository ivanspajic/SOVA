define([], function () {
    var getQuestions = async (callback) => {
        var response = await fetch("api/questions");
        var data = await response.json();
        callback(data);
    }

    var getQuestionByIdWithAnswers = async (questionId, callback) => {
        var response = await fetch(`api/questions/${questionId}`);
        var data = await response.json();
        callback(data);
    }
    return { getQuestions, getQuestionByIdWithAnswers };
});
