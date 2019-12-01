define(["knockout", "dataService"], function (ko, ds) {
    return function () {
        var questions = ko.observableArray([]);
        ds.getQuestions((data) => {
            questions(data.items.$values);
        });

        var activeComponent = ko.observable("landing-page");
        var activeParams = ko.observable({});
        var goToQuestionWithAnswer = () => {
            console.log("I am here");
            activeComponent("question-with-answers");
        };


        return {
            questions,
            activeComponent,
            activeParams,
            goToQuestionWithAnswer
        };

    };
});


