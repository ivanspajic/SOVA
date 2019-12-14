define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    var questionsByTag = ko.observable();
    var selectedTag = ko.observable(localStorage.getItem("selectedTag"));
    var prevPage = ko.observable();
    var nextPage = ko.observable();

    store.subscribe(() => {
        selectedTag(store.getState().selectedTag),
            ds.getQuestionsByTag((data) => {
                questionsByTag(data.items.$values);
                nextPage(data.next);
                prevPage(data.prev);
            });
    });

    ds.getQuestionsByTag((data) => {
        questionsByTag(data.items.$values);
        nextPage(data.next);
        prevPage(data.prev);
    });

    var getNext = function () {
        ds.getOtherPages(nextPage, (data) => {
            questionsByTag(data.items.$values);
            nextPage(data.next);
            prevPage(data.prev);
        });
    };

    var getPrev = function () {
        ds.getOtherPages(prevPage, (data) => {
            questionsByTag(data.items.$values);
            nextPage(data.next);
            prevPage(data.prev);
        });
    };

    var selectQuestion = (data, question) => {
        store.dispatch(store.actions.selectPost(question.questionId, true));
    };

    return function () {
        return {
            selectQuestion,
            selectedTag,
            questionsByTag,
            nextPage,
            prevPage,
            getNext,
            getPrev
        };
    };
});
