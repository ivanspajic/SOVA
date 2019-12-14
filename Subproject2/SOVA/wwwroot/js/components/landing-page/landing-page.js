define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    var questions = ko.observableArray([]);
    var sessionStorage = ko.observable(localStorage.getItem("username"));

    var prevPage = ko.observable();
    var nextPage = ko.observable();

    ds.getQuestions((data) => {
        questions(data.items.$values);
        nextPage(data.next);
        prevPage(data.prev);
    });

    var getNext = function () {
        ds.getOtherPages(nextPage, (data) => {
            questions(data.items.$values);
            nextPage(data.next);
            prevPage(data.prev);
        });
    };

    var getPrev = function () {
        ds.getOtherPages(prevPage, (data) => {
            questions(data.items.$values);
            nextPage(data.next);
            prevPage(data.prev);
        });
    };

    var selectQuestion = (data, submission) => {
        store.dispatch(store.actions.selectQuestion(submission.submissionId));
    }

    var selectQuestionsByTag = (data, tag) => {
        store.dispatch(store.actions.tagFilter(tag.tag.tagString));
    }
    return function () {
        return {
            questions,
            selectQuestion,
            selectQuestionsByTag,
            sessionStorage,
            nextPage,
            prevPage,
            getNext,
            getPrev
        };
    };
});
