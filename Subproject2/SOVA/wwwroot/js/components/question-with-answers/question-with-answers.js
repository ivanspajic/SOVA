define(["knockout", "dataService"], function (ko, ds) {
    return function () {
        var questionIdFromParams = window.location.split("/")[1];
        console.log(questionIdFromParams);
        var questionWithAnswer = ko.observable();
        ds.getQuestionByIdWithAnswers((questionIdFromParams, data) => {
            questionWithAnswer(questionIdFromParams, data);
        });

        return {
            questionWithAnswer
        };

    };
});