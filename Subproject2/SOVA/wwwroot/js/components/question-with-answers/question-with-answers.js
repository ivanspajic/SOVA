define(["knockout", "dataService"], function (ko, ds) {
    return function () {
        var questionIdFromParams = 19;

        var questionWithAnswer = ko.observable();
        ds.getQuestionByIdWithAnswers((data, questionIdFromParams) => {
            questionWithAnswer(data, questionIdFromParams);
        });

        return {
            questionIdFromParams,
            questionWithAnswer
        };

    };
});