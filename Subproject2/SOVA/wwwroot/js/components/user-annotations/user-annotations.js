define(['knockout', 'dataService', 'store'], function (ko, ds, store) {
    return function () {
        var allAnnotations = ko.observable();
        var prevPage = ko.observable();
        var nextPage = ko.observable();

        ds.getAllAnnotationsForUser((data) => {
            allAnnotations(data.items.$values);
            nextPage(data.next);
            prevPage(data.prev);
        });

        var selectQuestion = (data, question) => {
            store.dispatch(store.actions.selectQuestion(question.submissionId));
        };

        var getNext = function () {
            ds.getOtherPages(nextPage, (data) => {
                allAnnotations(data.items.$values);
                nextPage(data.next);
                prevPage(data.prev);
            });
        };

        var getPrev = function () {
            ds.getOtherPages(prevPage, (data) => {
                allAnnotations(data.items.$values);
                nextPage(data.next);
                prevPage(data.prev);
            });
        };

        return {
            allAnnotations,
            selectQuestion,
            getNext,
            getPrev,
            nextPage,
            prevPage
        }
    }
});