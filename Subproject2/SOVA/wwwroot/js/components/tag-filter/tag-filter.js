define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    var activeComponent = ko.observable("tag-filter");
    var questionsByTag = ko.observable();
    var selectedTag = ko.observable(store.getState().selectedTag);
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

    var selectQuestion = (data, questionId) => {
        store.dispatch(store.actions.selectPost(questionId, true));
    };

    return function () {
        return {
            activeComponent,
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
