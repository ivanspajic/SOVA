define(["knockout", "dataService"], function (ko, ds) {
    return function () {
        var questions = ko.observableArray([]);
        ds.getQuestions((data) => {
            questions(data.items.$values);
        });

        var currentComponent = ko.observable("landing-page");
        var currentParams = ko.observable({});
        var goToQuestionDetails = (questionId) => {
            currentParams({ name: `${questionId}` });
            currentComponent("question-with-answers");
        };

        return {
            currentParams,
            currentComponent,
            goToQuestionDetails,
            questions

        };

    };
});
