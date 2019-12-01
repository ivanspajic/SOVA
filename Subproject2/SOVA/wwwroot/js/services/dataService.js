define(["jquery"], function ($) {
    var getQuestions = (callback) => {
        $.getJSON("api/questions", callback);
    };
    var getQuestionAnswers = (questionId) => {
        $.getJSON(`api/questions/${questionId}`);
    }
    return { getQuestions, getQuestionAnswers };
});
