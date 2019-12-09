define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    var activeComponent = ko.observable("landing-page");
    var questions = ko.observableArray([]);

    ds.getQuestions((data) => {
        questions(data.items.$values);
    });

    var selectQuestion = (data, submission) => {
        store.dispatch(store.actions.selectQuestion(submission.submissionId));
    }

    return function () {
        return {
            activeComponent,
            questions,
            selectQuestion
        };
    };
});
