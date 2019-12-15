define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    return function () {
        var allAnnotations = ko.observable();

        ds.getAllAnnotationsForUser((data) => {
            allAnnotations(data.items.$values);
        });

        var selectPost = (data, question) => {
            store.dispatch(store.actions.selectQuestion(question.submissionId));
        };

        return {
            allAnnotations,
            selectPost
        }
    }
});