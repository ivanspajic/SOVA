define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    var questions = ko.observableArray([]);
    var sessionStorage = ko.observable(localStorage.getItem("username"));

    ds.getQuestions((data) => {
        questions(data.items.$values);
    });

    var selectQuestion = (data, submission) => {
        store.dispatch(store.actions.selectQuestion(submission.submissionId));
    }

    var selectQuestionsByTag = (data, tag) => {
        store.dispatch(store.actions.tagFilter(tag.tag.tagString));
    }
    var nextPage = () => {
        ds.moreQuestions((data) => {
            questions(data.items.$values);
        });
    }
    return function () {
        return {
            questions,
            selectQuestion,
            selectQuestionsByTag,
            sessionStorage,
            nextPage
        };
    };
});
