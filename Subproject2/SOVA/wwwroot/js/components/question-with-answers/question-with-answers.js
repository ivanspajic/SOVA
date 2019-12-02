define(["knockout", "dataService"], function (ko, ds) {
    return function () {
        var questionIdFromParams = 19;
        var questionWithAnswer = ko.observable();
        console.log(questionIdFromParams);
        ds.getQuestionByIdWithAnswers((data, questionIdFromParams) => {
            questionWithAnswer(data, questionIdFromParams);
        });

        return {
            questionIdFromParams,
            questionWithAnswer
        };

    };
});