define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    var activeComponent = ko.observable("tag-filter");
    var questionsByTag = ko.observableArray([]);

    store.subscribe(() => {
        ds.getQuestionsByTag((data) => {
            questionsByTag(data.items.$values);
        });
    });

    ds.getQuestionsByTag((data) => {
        questionsByTag(data.items.$values);
    });

    var selectQuestion = (data, questionId) => {
        store.dispatch(store.actions.selectPost(questionId, true));
    }

    return function () {
        return {
            activeComponent,
            selectQuestion,
            questionsByTag,
            selectQuestionByTag
        };
    };
});
