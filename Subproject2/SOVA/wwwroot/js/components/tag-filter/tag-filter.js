define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    var activeComponent = ko.observable("tag-filter");
    var questionsByTag = ko.observableArray([]);

    ds.getQuestionsByTag((data) => {
        questionsByTag(data.items.$values);
    });

    var selectQuestionByTag = (data, submission) => {
        //store.dispatch(store.actions.selectQuestionByTag(submission.submissionId));
    }

    return function () {
        return {
            activeComponent,
            questionsByTag,
            selectQuestionByTag
        };
    };
});
